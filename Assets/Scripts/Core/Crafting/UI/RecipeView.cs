using System.Collections.Generic;
using Data.Crafting.Container;
using Inventory.Container;
using UnityEngine;

namespace Core.Crafting.UI
{
    public abstract class RecipeView : MonoBehaviour
    {
        public InventoryObject Inventory;
        public GameObject SlotPrefab;
        public CraftingRecipeRegistry database;
        public CraftingStation Station;
        
        protected List<RecipeSlot> _slots;
        protected CraftSession _session;

        public abstract void CreateSlots();

        private void Start()
        {
            _session = new CraftSession(Station, Inventory);
            _session.Start();
            CreateSlots();
        }

        private void OnDestroy()
        {
            _session?.Stop();
        }

        public void OnSlotClicked(RecipeSlot slot)
        {
            if (_session.CanCraft(slot))
                _session.Craf(slot);
        }
    }
}