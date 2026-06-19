using System;
using Core.ToolMode;
using Inventory.Container;
using UnityEngine;

namespace Inventory.Core
{
    public class StaticInventoryView : InventoryView, IInventorySelectionSource 
    {
        public GameObject[] staticSlots;
        
        public event Action<InventorySlot> OnItemSelected;

        public override void CreateSlots()
        {
            var items = inventory.Container.Items;

            for (int i = 0; i < staticSlots.Length; i++)
            {
                SlotEventBinder.BindSlotEvent(staticSlots[i], items[i], this);
                SlotEventBinder.BindClickEvent(staticSlots[i], items[i], this);
                staticSlots[i].GetComponent<InventorySlotView>().Bind(items[i], inventory.database);
            }
        }
        
        public override void OnSlotClick(InventorySlot slot)
        {
            if (slot.item == null)
                return;
            
            OnItemSelected?.Invoke(slot);
        }
    }
}
