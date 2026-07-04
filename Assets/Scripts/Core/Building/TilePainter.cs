using System;               // нужен для Action (событие)
using Core.Building;        // нужен для TilemapLayerRegistry
using UnityEngine;          // Unity
using UnityEngine.EventSystems; // нужен для EventSystem (проверка: мышь над UI?)
using UnityEngine.Tilemaps; // нужен для Tilemap
using Zenject;              // нужен для [Inject]

namespace BuildSystem
{
    // Рисует и стирает тайлы на тайлмапе + управляет превью
    // Единственная ответственность: визуальная часть строительства
    // НЕ знает про инвентарь, предметы, режимы — только про тайлы
    public class TilePainter : MonoBehaviour
    {
        [SerializeField] private BuildInputHandler _inputHandler;   // мышь → координата клетки
        [SerializeField] private BuildPreviewSystem _previewSystem; // зелёный/красный квадрат под курсором

        private BuildTileData _activeTile;   // тайл зафиксированный кликом (активный)
        private BuildTileData _previewTile;  // тайл при наведении на кнопку в палитре (временный)
        private Tilemap _activeTilemap;      // тайлмап для активного тайла
        private Tilemap _previewTilemap;     // тайлмап для временного тайла
        private TilemapLayerRegistry _layerRegistry; // реестр тайлмапов

        // Событие: тайл поставлен — BuildModeController слушает и тратит предмет из инвентаря
        public event Action OnTilePlaced;

        // Zenject передаёт реестр тайлмапов
        [Inject]
        public void Construction(TilemapLayerRegistry layerRegistry)
        {
            _layerRegistry = layerRegistry;
        }

        // Устанавливает активный тайл — вызывается когда игрок выбрал предмет из инвентаря
        public void SetActiveTile(BuildTileData data, FlagsTilemapLayerType layerType)
        {
            _activeTile = data;                             // запоминаем тайл
            _activeTilemap = _layerRegistry.GetLayer(layerType); // находим нужный тайлмап
            _previewSystem.SetPreviewSprite(data.Icon);     // обновляем иконку превью
            _previewSystem.EnablePreview();                 // показываем превью
        }

        // Временный тайл при наведении мыши на кнопку в палитре
        public void PreviewTile(BuildTileData data, FlagsTilemapLayerType layerType)
        {
            _previewTile = data;
            _previewTilemap = _layerRegistry.GetLayer(layerType);
            _previewSystem.SetPreviewSprite(data.Icon);
            _previewSystem.EnablePreview();
        }

        // Восстанавливает превью после ухода мыши с кнопки палитры
        public void RestorePreview()
        {
            _previewTile = null;    // убираем временный тайл
            _previewTilemap = null;

            if (_activeTile)
                _previewSystem.SetPreviewSprite(_activeTile.Icon); // возвращаем иконку активного
            else
                _previewSystem.DisablePreview(); // нечего показывать
        }

        // Очищает всё — вызывается при выходе из режима строительства
        public void ClearActiveTile()
        {
            _activeTile = null;
            _previewTile = null;
            _activeTilemap = null;
            _previewTilemap = null;
            _previewSystem.DisablePreview(); // прячем превью
        }

        // Update — каждый кадр
        private void Update()
        {
            if (!_activeTile) // нет активного тайла — ничего не делаем
                return;

            // IsPointerOverGameObject — мышь над UI элементом (кнопкой, панелью)?
            if (EventSystem.current.IsPointerOverGameObject())
            {
                _previewSystem.DisablePreview(); // прячем превью под UI
                return;
            }

            _previewSystem.EnablePreview(); // показываем превью

            Vector3Int cell = _inputHandler.GetMouseCellPosition(); // клетка под мышью

            // Если мышь над кнопкой палитры — используем previewTile
            // Иначе — используем activeTile
            // ? : — тернарный оператор: условие ? если_да : если_нет
            BuildTileData current = _activeTile ? _activeTile : _previewTile;
            Tilemap currentLayer  = _activeTilemap ? _activeTilemap : _previewTilemap;

            // Обновляем цвет превью: зелёный = можно, красный = занято
            _previewSystem.UpdatePreview(cell, CanPaint(cell, currentLayer));

            // Левая кнопка мыши + клетка свободна → рисуем тайл
            if (Input.GetMouseButtonDown(0) && CanPaint(cell, currentLayer))
                Paint(cell, current, currentLayer);

            // Правая кнопка мыши → стираем тайл
            if (Input.GetMouseButton(1))
                Erase(cell, currentLayer);
        }

        // Проверяет: можно ли поставить тайл в эту клетку
        // layer != null И на клетке нет тайла
        private bool CanPaint(Vector3Int cell, Tilemap layer) =>
            layer && !layer.GetTile(cell);

        // Ставит тайл и вызывает событие (инвентарь потратит предмет)
        private void Paint(Vector3Int cell, BuildTileData data, Tilemap layer)
        {
            layer.SetTile(cell, data.Tile); // ставим тайл
            OnTilePlaced?.Invoke();          // сообщаем что тайл поставлен
        }

        // Убирает тайл с клетки (null = пусто)
        private void Erase(Vector3Int cell, Tilemap layer) =>
            layer.SetTile(cell, null);
    }
}