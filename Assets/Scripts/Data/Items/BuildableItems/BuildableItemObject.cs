using BuildSystem;  // нужен для BuildPalette
using UnityEngine;  // Unity

namespace Inventory.Item
{
    // Строительный предмет — например "Забор" или "Грядка"
    // Наследует ItemObject (базовые данные предмета)
    // Реализует IBuildable (знает какую палитру открыть при выборе)
    [CreateAssetMenu(fileName = "New Buildable Item", menuName = "Inventory System/Items/Buildable")]
    public class BuildableItemObject : ItemObject, IBuildable
    {
        // Какая палитра тайлов открывается когда выбираем этот предмет
        // Например предмет "Забор" → открывает палитру со всеми видами заборов
        [field: SerializeField] public BuildPalette LinkedPalette { get; private set; }

        // Какой тайл из палитры выбрать сразу по умолчанию (индекс в массиве)
        // 0 = первый тайл в палитре
        [field: SerializeField] public int DefaultTileIndex { get; private set; }
    }
}