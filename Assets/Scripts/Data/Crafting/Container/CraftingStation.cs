using System;
using System.Collections.Generic;
using Inventory.Container;
using UnityEngine;

namespace Data.Crafting.Container
{
    [CreateAssetMenu(fileName = "New Crafting Station", menuName = "Crafting System/Station")]
    public class CraftingStation : ScriptableObject
    {
        public CraftingRecipeRegistry database;
        public Recipe container;
        public CraftingStationType stationType;
    }


    [Serializable]
    public class Recipe
    {
        public List<RecipeSlot> RecipeItems; //хранит все доступные рецепты для конкретной станции

        [field:SerializeField] public event Action<RecipeSlot> OnRecipeAdded;
        
        public void Initialize()
        {
            RecipeItems = new List<RecipeSlot>();
        }

        public void AddRecipe(RecipeSlot recipe) //открывается новый рецепт за баранки
        {
            if(RecipeItems.Contains(recipe))
                return;

            /*foreach (var existing in RecipeItems)
            {
                if (existing.recipe.Id == recipe.recipe.Id)
                {
                    return;
                }
            }*/
            
            RecipeItems.Add(recipe);
            OnRecipeAdded?.Invoke(recipe);
        }
    }

    [Serializable]
    public class RecipeSlot //конкретный рецепт в конкретной ячейке в нашей системе. на какой рецепт
        //кликнул наш пользак
    {
        public CraftingRecipe recipe;
        public CraftingStationType station;
        public bool bIsAvailable;

        [field: SerializeField] public event Action<bool> OnIsAvailableChanged;

        public RecipeSlot(CraftingRecipe recipe, CraftingStationType station)
        {
            this.recipe = recipe;
            this.station = station;
        }

        public void SetAvailable(bool value)
        {
            if(value = bIsAvailable)
                return;
            bIsAvailable = value;
        }
    }
}