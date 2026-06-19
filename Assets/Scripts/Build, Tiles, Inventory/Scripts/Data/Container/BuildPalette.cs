using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildSystem
{
    [CreateAssetMenu(fileName = "BuildPalette", menuName = "Build System/Palette")]
    public class BuildPalette : ScriptableObject
    {
        [field: SerializeField] public int Id;
        [field: SerializeField] public string PaletteName            { get; private set; }
        [field: SerializeField] public TilemapLayerType LayerType    { get; private set; }
        [field: SerializeField] public BuildTileData[] Tiles         { get; private set; }
        [field: SerializeField] public bool IsUnlocked               { get; private set; }

        // Разблокировка в рантайме (квест, прогресс и т.д.)
        public void Unlock() => IsUnlocked = true;
        public void Lock()   => IsUnlocked = false;
    }
}