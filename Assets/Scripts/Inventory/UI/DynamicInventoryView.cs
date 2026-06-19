using System;
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
        public GameObject sellPrefab;
        public SlotPosition SlotPosition;
        
        private InventorySlotRenderer _renderer;
        private ISlotSource _source;
        public override void CreateSlots()
        {
            _renderer = new InventorySlotRenderer(inventory);
            _source = new DynamicSlotSource(SlotPosition);

            itemsDisplay = _renderer.CreateSlots(
                    _source,
                    sellPrefab,
                    transform,
                    this);
        }

        public override void RefreshUI()
        {
            _renderer.UpdateSlots();
        }

       
    }
