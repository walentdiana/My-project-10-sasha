using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildSystem
{
    [CreateAssetMenu(fileName = "TileData", menuName = "Build System/Tile Data")]
    public class BuildTileData : ScriptableObject
    {
        [field:  SerializeField] public string TileName { get; private set; }
        [field: SerializeField] public TileBase Tile    { get; private set; }
        [field: SerializeField] public Sprite Icon      { get; private set; }
    }
}
