using System; // нужен для [Flags]

namespace Inventory.Item
{
    // [Flags] — те же битовые флаги что и ItemCategory
    // Описывает ЧТО УМЕЕТ делать инструмент
    // Лопата: Till | Loosen — и копает и рыхлит
    // Топор: только Chop
    // Лейка: только Water
    [Flags]
    public enum ToolCapability
    {
        None   = 0,       // инструмент ничего не умеет
        Till   = 1 << 0,  // перекапывает землю (лопата)
        Loosen = 1 << 1,  // рыхлит почву
        Water  = 1 << 2,  // поливает (лейка)
        Chop   = 1 << 3,  // рубит деревья (топор)
    }
}