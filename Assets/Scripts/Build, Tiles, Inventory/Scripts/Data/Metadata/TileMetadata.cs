using BuildSystem;
using Inventory.Item;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data.Metadata
{
    [CreateAssetMenu(fileName = "TileMetadata", menuName = "Build System/Tile Metadata")]
    public class TileMetadata : ScriptableObject
    {
        public TilemapLayerType SourceLayer;
        public ToolCapability RequiredCapability; // трава требует Till, дерево — Chop
        public TileBase ResultTile;              // во что превращается
        public TilemapLayerType ResultLayer;

        // Следующее состояние по времени (заглушка — система времени подключится позже)
        public TileMetadata NextState;        // политая грядка → грядка (через время)
        public float TimeToNextState = -1f;   // -1 = не меняется само по себе
    }
}