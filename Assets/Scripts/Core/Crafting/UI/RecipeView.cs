using System;
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
        
        public event Action<RecipeSlot,CraftSession> OnRecipeSelected;
        public abstract void CreateSlots();

        private void Start()
        {
            _session = new CraftSession(Station, Inventory);
            _session.CalculateItem();
            CreateSlots();
        }
        

        public void OnSlotClicked(RecipeSlot slot)
        {
            if(!slot.bIsAvailable)
                return;
            
            OnRecipeSelected?.Invoke(slot,_session);
        }
    }
}