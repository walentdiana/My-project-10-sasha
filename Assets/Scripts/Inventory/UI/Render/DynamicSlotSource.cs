using Inventory.Container;
using UnityEngine;

    public class DynamicSlotSource : ISlotSource
    {
        private SlotPosition _position;

        public DynamicSlotSource(SlotPosition position)
        {
            _position = position;
        }

        public GameObject[] CreateSlots(GameObject prefab, Transform root, InventoryObject inventory)
        {
            var items = inventory.Container.Items;
            var result = new GameObject[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                var obj = Object.Instantiate(prefab, root);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(
                    _position.x_Start + (_position.x_Space_Between_Item * (i % _position.number_Of_Column)),
                    _position.y_Start + (-_position.y_Space_Between_Item * (i / _position.number_Of_Column)),
                    0f);
                result[i] = obj;
            }
            return result;
        }
    }
