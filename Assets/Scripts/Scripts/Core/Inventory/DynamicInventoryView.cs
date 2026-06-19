using System;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public struct SlotPosition
{
    public int x_Start,  y_Start;
    public int x_Space_Between_Item, y_Space_Between_Item;
    public int number_Of_Column;
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
                SlotPosition.x_Start + (SlotPosition.x_Space_Between_Item * (i % SlotPosition.number_Of_Column)),
                SlotPosition.y_Start + (-SlotPosition.y_Space_Between_Item * (i / SlotPosition.number_Of_Column)),
                0f
            );
        }
    }
