using UnityEngine;        // Unity
using UnityEngine.Tilemaps; // нужен для Tilemap (хотя тут не используется напрямую)

namespace BuildSystem
{
    // Палитра строительных тайлов — например "все виды заборов"
    // ScriptableObject — хранится как .asset файл
    // BuildableItemObject ссылается на неё: "этот предмет открывает эту палитру"
    [CreateAssetMenu(fileName = "BuildPalette", menuName = "Build System/Palette")]
    public class BuildPalette : ScriptableObject
    {
        // [field: SerializeField] — сериализует само свойство (без отдельного поля)
        // { get; private set; } — читать можно снаружи, менять только внутри класса
        [field: SerializeField] public int Id; // уникальный номер палитры (проставляет PaletteDatabase)

        [field: SerializeField] public string PaletteName { get; private set; } // название ("Fences", "Gardens")

        [field: SerializeField] public FlagsTilemapLayerType LayerType { get; private set; } // на каком слое рисовать

        [field: SerializeField] public BuildTileData[] Tiles { get; private set; } // массив тайлов этой палитры

        [field: SerializeField] public bool IsUnlocked { get; private set; } // открыта ли палитра игроку

        // Разблокировать палитру (квест выполнен, достижение получено и т.д.)
        public void Unlock() => IsUnlocked = true;  // => это сокращение: { IsUnlocked = true; }

        // Заблокировать палитру (например при сбросе прогресса)
        public void Lock() => IsUnlocked = false;
    }
}