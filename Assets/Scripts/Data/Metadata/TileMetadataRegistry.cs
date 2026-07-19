using System.Collections.Generic; // нужен для Dictionary
using System.Linq;                 // нужен для OrderByDescending
using Core.Building;               // нужен для TilemapLayerRegistry
using UnityEngine;                 // Unity
using UnityEngine.Tilemaps;       // нужен для TileBase, Tilemap

namespace Data.Metadata
{
    // Одна запись в реестре: несколько вариантов тайла → одни метаданные
    // Пример: трава светлая, трава тёмная, трава с камнями
    //         → все три требуют лопату и дают грядку
    [System.Serializable]
    public class TileMetadataEntry
    {
        public TileBase[] Tiles;     // массив тайлов (разные варианты одного типа)
        public TileMetadata Metadata; // метаданные для всех этих тайлов
    }

    // Реестр всех метаданных тайлов — один .asset файл на весь проект
    // ScriptableObject — заполняется в инспекторе, передаётся через Zenject
    [CreateAssetMenu(fileName = "TileMetadataRegistry", menuName = "Build System/Tile Metadata Registry")]
    public class TileMetadataRegistry : ScriptableObject
    {
        [SerializeField] private TileMetadataEntry[] _entries; // все записи (заполняется в инспекторе)

        private TileMetadataEntry[] _sortedEntries; // отсортированные записи (по приоритету слоя)

        // Словарь для быстрого поиска: тайл → метаданные
        private Dictionary<TileBase, TileMetadata> _cache;

        // Инициализация — строит кэш и сортирует записи
        // Вызывается через TileMetadataRegistryInitializer при старте игры
        public void Initialize()
        {
            _cache = new Dictionary<TileBase, TileMetadata>(); // пустой словарь

            foreach (var entry in _entries)
            {
                foreach (var tile in entry.Tiles) // для каждого варианта тайла
                {
                    if (!tile) continue; // пропускаем пустые слоты в массиве

                    _cache[tile] = entry.Metadata; // тайл → метаданные
                }
            }

            // Сортируем по убыванию приоритета слоя (Buildings=4 проверяется раньше Ground=0)
            // Чтобы тайл на верхнем слое не "перекрывался" тайлом на нижнем
            // OrderByDescending — от большего к меньшему
            // ToArray() — превращаем в массив (LINQ возвращает IEnumerable)
            _sortedEntries = _entries
                .OrderByDescending(e => (int)e.Metadata.SourceLayer)
                .ToArray();
        }

        // Ищет метаданные для тайла в клетке cell
        // Проверяет слои сверху вниз (по приоритету)
        // out TileMetadata metadata — выходной параметр: найденные метаданные
        public bool TryGetMetadataAtCell(
            Vector3Int cell,                    // координата клетки
            TilemapLayerRegistry layerRegistry, // реестр тайлмапов
            out TileMetadata metadata)          // результат поиска
        {
            foreach (var entry in _sortedEntries) // перебираем слои от верхнего к нижнему
            {
                // Получаем Tilemap для этого слоя
                if (!layerRegistry.TryGetLayer(entry.Metadata.SourceLayer, out Tilemap layer))
                {
                    Debug.Log($"Layer {entry.Metadata.SourceLayer} not found in registry");
                    continue; // нет такого слоя — пропускаем
                }

                TileBase tile = layer.GetTile(cell); // что стоит на этой клетке?
                Debug.Log($"Checking layer {entry.Metadata.SourceLayer}: tile = {(tile ? tile.name : "null")}");

                // Тайл есть И он есть в нашем словаре?
                if (tile && _cache.TryGetValue(tile, out metadata))
                    return true; // нашли метаданные!
            }

            metadata = null; // ничего не нашли
            return false;
        }
    }
}