using System.Collections.Generic;
using BuildSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.Building
{
    public class TilemapLayerRegistry : MonoBehaviour
    {
        private readonly Dictionary<TilemapLayerType, Tilemap> _layers = new();
        
        public void Register(TilemapLayerType type, Tilemap tilemap)
        {
            if (_layers.ContainsKey(type))
            {
                Debug.LogWarning($"TilemapLayerRegistry: layer {type} already registered, overwriting.");
                _layers[type] = tilemap;
                return;
            }
            _layers[type] = tilemap;
        }
        
        public bool TryGetLayer(TilemapLayerType type, out Tilemap tilemap)
            => _layers.TryGetValue(type, out tilemap);

        public Tilemap GetLayer(TilemapLayerType type)
        {
            if (_layers.TryGetValue(type, out var tilemap))
                return tilemap;

            Debug.LogError($"TilemapLayerRegistry: layer {type} not found.");
            return null;
        }
    }
}