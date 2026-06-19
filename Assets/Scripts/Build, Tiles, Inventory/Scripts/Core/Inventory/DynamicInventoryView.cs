using System;
using UnityEngine;

namespace Inventory.Core
{
    
    [Serializable]
    public struct SlotPosition 
    {
        public int X_START;
        public int Y_START;
        public int X_SPACE_BETWEEN_ITEM;
        public int NUMBER_OF_COLUMN;
        public int Y_SPACE_BETWEEN_ITEMS;
    }
    
    public class DynamicInventoryView : InventoryView
    {
        public SlotPosition SlotPosition;

        public override void CreateSlots()
        {
            slots = inventory.Container.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                
                var obj = Instantiate(slotPrefab, transform);
                obj.GetComponent<RectTransform>().localPosition = CalculatePos(i);
                SlotEventBinder.BindSlotEvent(obj, slots[i], this);
                
                var view = obj.GetComponent<InventorySlotView>();
                view.Bind(slots[i], inventory.database);
            }
        }

        private Vector3 CalculatePos(int i)
        {
            return new Vector3(
                SlotPosition.X_START + (SlotPosition.X_SPACE_BETWEEN_ITEM * (i % SlotPosition.NUMBER_OF_COLUMN)),
                SlotPosition.Y_START + (-SlotPosition.Y_SPACE_BETWEEN_ITEMS * (i / SlotPosition.NUMBER_OF_COLUMN)),
                0f
            );
        }
    }
}
