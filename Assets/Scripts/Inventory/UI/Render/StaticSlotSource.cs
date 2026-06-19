using Inventory.Container;
using UnityEngine;

    public class StaticSlotSource : ISlotSource
    {
        private GameObject[] _slots;

        public StaticSlotSource(GameObject[] slots)
        {
            _slots = slots;
        }

        public GameObject[] CreateSlots(GameObject prefab, Transform root, InventoryObject inventory)
        {
            return _slots;
        }
    }
