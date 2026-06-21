using System;              // нужен для Action
using Inventory.Container; // нужен для InventorySlot

namespace Core.ToolMode
{
    // Интерфейс: "я умею сообщать о том что игрок выбрал предмет"
    // StaticInventoryView реализует его
    // BuildModeController и ToolModeController подписываются на событие
    //
    // Зачем интерфейс? Чтобы контроллеры не зависели напрямую от StaticInventoryView
    // Завтра можно создать другой источник выбора предметов (геймпад, автовыбор)
    // и контроллеры не нужно будет менять
    public interface IInventorySelectionSource
    {
        // Событие: игрок кликнул на ячейку, передаём эту ячейку подписчикам
        event Action<InventorySlot> OnItemSelected;
    }
}