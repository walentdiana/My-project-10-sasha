using Data.Metadata;
using UnityEngine;

namespace BuildSystem.TileTimeDependent
{
    // Заглушка — реализация появится когда придёт система времени
    public interface ITileTimeDependent
    {
        void RegisterTimedTile(Vector3Int cell, TilemapLayerType layer, TileMetadata metadata);
        void UnregisterTimedTile(Vector3Int cell, TilemapLayerType layer);
    }
    
    public class TileTimeDependentStub : ITileTimeDependent
    {
        public void RegisterTimedTile(Vector3Int cell, TilemapLayerType layer, TileMetadata metadata)
        {
            // TODO: система времени
        }
        
        public void UnregisterTimedTile(Vector3Int cell, TilemapLayerType layer)
        {
            // TODO: система времени
        }
    }
}