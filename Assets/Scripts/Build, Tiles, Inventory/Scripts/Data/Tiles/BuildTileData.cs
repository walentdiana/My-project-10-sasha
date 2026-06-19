using UnityEngine;          // Unity
using UnityEngine.Tilemaps; // нужен для TileBase

namespace BuildSystem
{
    // Данные одного тайла в палитре
    // ScriptableObject — хранится как .asset
    // Пример: тайл "деревянный забор" — у него есть спрайт и сам тайл для тайлмапа
    [CreateAssetMenu(fileName = "TileData", menuName = "Build System/Tile Data")]
    public class BuildTileData : ScriptableObject
    {
        [field: SerializeField] public string TileName { get; private set; } // название тайла ("Wood Fence")

        [field: SerializeField] public TileBase Tile { get; private set; } // сам тайл Unity — ставится на тайлмап

        [field: SerializeField] public Sprite Icon { get; private set; } // иконка для кнопки в UI палитры
    }
}