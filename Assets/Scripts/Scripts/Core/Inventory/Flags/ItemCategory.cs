using System;

namespace Core.Inventory.Flags
{
    [Flags]
    public enum ItemCategory
    {
        None       = 0,         //0000 0000
        Weapon     = 1 << 0,   //0000 0001
        Armor      = 1 << 1,
        Food       = 1 << 2,
        Consumable = 1 << 3,
        Buildable  = 1 << 4,
        Material   = 1 << 5,
        Tools      = 1 << 6     // 0100 0000  
    }
}