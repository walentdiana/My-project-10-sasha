using System;

namespace Inventory.Item
{
    [Flags]
    public enum ToolCapability
    {
        None     = 0,
        Till     = 1 << 0,  // перекапывает
        Loosen   = 1 << 1,  // рыхлит
        Water    = 1 << 2,  // поливает
        Chop     = 1 << 3,  // рубит
    }
}