using System;
using Inventory.Container;

namespace Core.ToolMode
{
    public interface IInventorySelectionSource
    {
        event Action<InventorySlot> OnItemSelected;
    }
}