using System.Collections.Generic;
using System.Linq;
using Core.Building;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data.Metadata
{
    [System.Serializable]
    public class TileMetadataEntry
    {
        public TileBase[] Tiles;       // все варианты травы
        public TileMetadata Metadata;  // одни метаданные для всех
    }
    
    [CreateAssetMenu(fileName = "TileMetadataRegistry", menuName = "Build System/Tile Metadata Registry")]
    public class TileMetadataRegistry : ScriptableObject
    {
        [SerializeField] private TileMetadataEntry[] _entries;
        private TileMetadataEntry[] _sortedEntries;

        private Dictionary<TileBase, TileMetadata> _cache;

        public void Initialize()
        {
            _cache = new Dictionary<TileBase, TileMetadata>();
            foreach (var entry in _entries)
            {
                foreach (var tile in entry.Tiles)
                {
                    if (!tile)
                    {
                        continue;
                    }
                    _cache[tile] = entry.Metadata;
                }
            }

            // сортируем по убыванию приоритета слоя — один раз
            _sortedEntries = _entries
                .OrderByDescending(e => (int)e.Metadata.SourceLayer)
                .ToArray();
        }
        
        public bool TryGetMetadataAtCell(
            Vector3Int cell,
            TilemapLayerRegistry layerRegistry,
            out TileMetadata metadata)
        {
            foreach (var entry in _sortedEntries)
            {
                if (!layerRegistry.TryGetLayer(entry.Metadata.SourceLayer, out Tilemap layer))
                    continue;

                TileBase tile = layer.GetTile(cell);
                
                if (tile && _cache.TryGetValue(tile, out metadata))
                    return true;
            }

            metadata = null;
            return false;
        }
    }
}