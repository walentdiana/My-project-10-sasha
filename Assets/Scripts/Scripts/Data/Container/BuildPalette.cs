using UnityEngine;

namespace BuildSystem
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Build System/Palette")]
    public class BuildPalette : ScriptableObject
    {
        public int Id; 
        [field: SerializeField] public string PaletteName    { get; private set; }
        [field: SerializeField] public BuildTileData[] Tiles { get; private set; }
        [field: SerializeField] public TilemapLayerType Type { get; private set; }
        [field: SerializeField] public bool bIsUnloked       { get; private set; }
        
        public void Unlock() => bIsUnloked = true;
        public void Lock() => bIsUnloked   = false;
    }
}