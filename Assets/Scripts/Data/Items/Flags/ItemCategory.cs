using System; // нужен для атрибута [Flags]

namespace Inventory.Item // папка в коде
{
    // [Flags] — говорит C#: этот enum используется как набор битов
    // Это значит можно комбинировать: AllowedCategories = Weapon | Tool
    // Проверка: (AllowedCategories & item.Category) != 0
    //
    // Как работают биты:
    // None      = 0000 0000
    // Weapon    = 0000 0001  (бит 0)
    // Armor     = 0000 0010  (бит 1)
    // Consumable= 0000 0100  (бит 2)
    // и т.д.
    // Weapon | Armor = 0000 0011 — значит "разрешены оружие ИЛИ броня"
    [Flags]
    public enum ItemCategory
    {
        None       = 0,       // нет категории (нет ограничений на слот)
        Weapon     = 1 << 0,  // оружие    — 1 сдвинуть на 0 бит = 1
        Armor      = 1 << 1,  // броня     — 1 сдвинуть на 1 бит = 2
        Consumable = 1 << 2,  // расходник — 1 сдвинуть на 2 бит = 4
        Buildable  = 1 << 3,  // строительный предмет             = 8
        Material   = 1 << 4,  // материал                         = 16
        Tool       = 1 << 5   // инструмент                       = 32
    }
}