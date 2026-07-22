using System.Collections.Generic;
using Data.Crafting.Container;
using Inventory.Container;
using UnityEngine;

namespace Core.Crafting
{
    public class CraftSession
    {
        private readonly CraftingStation _station;
        private readonly  InventoryObject _inventory;
        private readonly Dictionary<int, int> _itemCounts = new();

        public CraftSession(CraftingStation station, InventoryObject inventory)  //если класс не монобез и не so, то передать параметры моем только через конструктор 
        {
            _station = station;
            _inventory = inventory;
        }

        public void CalculateItem()
        {
            _itemCounts.Clear();
            foreach (var slot in _inventory.Container.Items)
            {
                if(slot.ID < 0)
                    continue;
                
                _itemCounts[slot.item.Id] = _itemCounts.GetValueOrDefault(slot.item.Id) + slot.amount;
                CalculateRecipe();
            }
            
        }

        private void CalculateRecipe()
        {
            foreach (var recipeSlot in _station.Container.RecipeItems)
                recipeSlot.SetAvailable(CanCraft(recipeSlot));
        }

        public bool CanCraft(RecipeSlot slot, int craftingAmount = 1)
        {
            foreach (var ingredient in slot.recipe.ItemIngredients)
            {
                if (_itemCounts.GetValueOrDefault(ingredient.Item.Id) < ingredient.Amount * craftingAmount) 
                    return false;
            }
            
            return true;
        }
        

        private void OnRecipeAdded(RecipeSlot slot)
        {
           slot.SetAvailable(CanCraft(slot));
        }

        private void RecalculateAll()
        {
            foreach (var slot in _station.Container.RecipeItems)
                slot.SetAvailable(CanCraft(slot));
        }

        public void RecalculateCurrentRecipe(RecipeSlot slot, int value) //пересчет одного конкретного рецепта
        {
            slot.SetAvailable(CanCraft(slot, value));
        }
        
        public int GetItemCount(int itemId) => _itemCounts.GetValueOrDefault(itemId); //одает количество предметов по id item словаря

        public bool Craft(RecipeSlot slot, int value)
        {
           if (!CanCraft(slot))
               return false;

           foreach (var ingredient in slot.recipe.ItemIngredients)
           {
               int ingredientAmount = ingredient.Amount * value;
               ConsumeItem(ingredient.Item.Id, ingredientAmount);
               _itemCounts[ingredient.Item.Id] -= ingredientAmount;
           }

           var result = slot.recipe.ItemResult.CreateItem();
           int resultAmount = slot.recipe.ResultAmount * value;
           _inventory.AddItem(result,resultAmount);
           _itemCounts[result.Id] = _itemCounts.GetValueOrDefault(result.Id) + resultAmount;
           CalculateItem();
           return true;
        }
        

        private void ConsumeItem(int itemId, int amount) //при крафте удаляет предметы из инвентаря для крафта 
        {
            int remaining =  amount;

            foreach (var slot in _inventory.Container.Items)
            {
                if (remaining < 0)
                    break;
                
                if (slot.ID != itemId)
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