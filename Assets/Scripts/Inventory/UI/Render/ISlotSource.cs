
    using Inventory.Container;
    using UnityEngine;

    public interface ISlotSource
    {
        GameObject[] CreateSlots(
            GameObject prefab,
            Transform root,
            InventoryObject inventory);
    }
