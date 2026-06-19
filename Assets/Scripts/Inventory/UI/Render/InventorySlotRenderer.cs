using System.Collections.Generic;
using Inventory;
using Inventory.Container;
using Inventory.UI;
using UnityEngine;

    public class InventorySlotRenderer
    {
        private Dictionary<GameObject, InventorySlot> _itemDisplayed;
        private InventoryObject _inventory;

        public InventorySlotRenderer(InventoryObject inventory)
        {
            _inventory = inventory;
            _itemDisplayed = new Dictionary<GameObject, InventorySlot>();
        }

        public Dictionary<GameObject, InventorySlot> CreateSlots(
            ISlotSource source,
            GameObject prefab, 
            Transform root, 
            InventoryView parent)
        {
            _itemDisplayed.Clear();
            var slots = source.CreateSlots(prefab, root, _inventory);
            var items = _inventory.Container.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                SlotEventBinder.BindSlotEvents(slots[i], parent);
                items[i].parent = parent;
                _itemDisplayed.Add(slots[i], items[i]);
            }
            return _itemDisplayed;
        }

        public void UpdateSlots()
        {
            foreach (var slot in _itemDisplayed)
            {
                var view = slot.Key.GetComponent<InventorySlotView>();
                view.Refresh(slot.Value, _inventory.database);
            }
        }
        
    }
