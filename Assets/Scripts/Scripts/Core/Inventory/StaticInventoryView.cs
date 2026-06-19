using System;
using BuildSystem;
using Core.Building;
using Core.Inventory;
using Inventory;
using Inventory.Container;
using UnityEngine;


public class StaticInventoryView : InventoryView, IBuildRequestSource
    {
        public GameObject[] staticSlots;
        public event Action<BuildPalette, int> OnBuild;

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
            if (slot.item is IBuildable buildable)
                OnBuild?.Invoke(buildable.LinkedPalette, buildable.DefaultTileIndex);
        }
    }
