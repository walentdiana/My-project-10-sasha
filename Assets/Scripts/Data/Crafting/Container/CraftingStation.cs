using System;
using System.Collections.Generic;
using Inventory.Container;
using UnityEngine;

namespace Data.Crafting.Container
{
    [CreateAssetMenu(fileName = "New Crafting Station", menuName = "Crafting System/Station")]
    public class CraftingStation : ScriptableObject
    {
        public CraftingRecipeRegistry registry;
        public Recipe container;

        public bool CanBeCrafted(RecipeSlot recipe, InventoryObject  inventory)
        {
            foreach (var ingredient in recipe.recipe.ItemIngredients)
            {
                int have = CountInInventory(inventory, ingredient);
                    if (have < ingredient.Amount)
                        return false;
            }
            return true;
        }

        private int CountInInventory(InventoryObject inventory, CraftingIngredients ingredient)
        {
            int total = 0;

            foreach (var slot in inventory.Container.Items)
            {
                if (slot.ID >= 0
                    && slot.ID != null
                    && slot.ID == ingredient.Item.Id)
                    total += slot.amount;
            }
            return total;
        }

        public bool Craft()
        {
            return true;
        }
    }


    [Serializable]
    public class Recipe
    {
        public List<RecipeSlot> RecipeItems; //хранит все рецепты для конкретной станции

        public void Initialize()
        {
            RecipeItems = new List<RecipeSlot>();
        }

        public void AddRecipe(RecipeSlot slot) //открывается новый рецепт за баранки
        {
            if(RecipeItems.Contains(slot))
                return;
            
            RecipeItems.Add(slot);
        }
    }

    [Serializable]
    public class RecipeSlot
    {
        public CraftingRecipe recipe;
        public CraftingStationType station;
    }
}