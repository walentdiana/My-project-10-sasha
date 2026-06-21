using BuildSystem; // нужен для BuildPalette

namespace Inventory.Item
{
    // interface — контракт для строительных предметов
    // BuildModeController проверяет: slot.item.Source is IBuildable?
    // Если да — активирует режим строительства с нужной палитрой
    public interface IBuildable
    {
        BuildPalette LinkedPalette { get; }  // какая палитра тайлов привязана к этому предмету
        int DefaultTileIndex { get; }        // какой тайл из палитры выбрать по умолчанию
    }
}