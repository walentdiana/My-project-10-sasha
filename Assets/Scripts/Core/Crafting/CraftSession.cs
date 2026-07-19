using Data.Crafting;
using Data.Crafting.Container;
using Inventory.Container;
using UnityEngine;

namespace Core.Crafting
{
    public class CraftSession
    {
        private readonly CraftingStation _station;
        private readonly  InventoryObject _inventory;

        public CraftSession(CraftingStation station, InventoryObject inventory)  //если класс не монобез и не so, то передать параметры моем только через конструктор 
        {
            _station = station;
            _inventory = inventory;
        }

        public void Start()
        {
            foreach (var slot in _inventory.Container.Items)
                slot.OnChanged += RecalculateAll;

            _station.Container.OnRecipeAdded += OnResipeAdded;
            RecalculateAll();
        }

        public void Stop()
        {
            foreach (var slot in _inventory.Container.Items)
                slot.OnChanged -= RecalculateAll;

            _station.Container.OnRecipeAdded -= OnResipeAdded;
        }

        public bool CanCraft(RecipeSlot slot)
        {
            foreach (var ingredient in slot.recipe.ItemIngredients)
                if(!IsSatisfied(ingredient))
                    return false;
            
            return true;
        }

        private bool IsSatisfied(CraftingIngredients ingredient) //Проверяет один конкретный ингредиент:
        {
            int total = 0;

            foreach (var invSlot in _inventory.Container.Items)
                if (invSlot.item != null && invSlot.item.Id == ingredient.Item.Id)
                    total += invSlot.amount;

            return total >= ingredient.Amount;
        }

        private void OnResipeAdded(RecipeSlot slot)
        {
           slot.SetAvailable(CanCraft(slot));
        }

        private void RecalculateAll()
        {
            foreach (var slot in _station.Container.RecipeItems)
                slot.SetAvailable(CanCraft(slot));
        }

        public bool Craf(RecipeSlot slot)
        {
            if(!CanCraft(slot))
                return false;

            foreach (var ingredient in slot.recipe.ItemIngredients)
                ConsumeItem(ingredient);

            var resultItem = slot.recipe.ItemResult.CreateItem();
            _inventory.AddItem(resultItem, slot.recipe.ResultAmount);
            return true;
        }
        

        private void ConsumeItem(CraftingIngredients ingredient) //при крафте удаляет предметы из инвентаря для крафта 
        {
            int remaining =  ingredient.Amount;

            foreach (var slot in _inventory.Container.Items)
            {
                if (remaining < 0)
                    break;
                
                if (slot.item == null || slot.item.Id != ingredient.Item.Id)
                    continue;
                
                int toRemove = Mathf.Min(slot.amount, remaining);
                
                if(slot.amount - toRemove <= 0)
                    slot.UpdateSlot(-1,null,0);
                else
                    slot.AddAmount(-toRemove);
                
                remaining -= toRemove;
            }
        }
    }
}