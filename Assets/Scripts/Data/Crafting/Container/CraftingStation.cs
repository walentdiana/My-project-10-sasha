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
        public Recipe Container;
        public CraftingStationType stationType;

        private void OnEnable()  //если список рецептов ещё не создан — создаём пустой список.
        {
            if(Container.RecipeItems == null)
                Container.Initialize();
        }


        private void UnlockRecipe(int recipeId)
        {
            if (!database.TryGetRecipe(recipeId, out var recipe)) //Ищем рецепт в базе по id. Не нашли — выходим.
                return;
            
            if((recipe.CraftingStationType &  stationType) == 0) //Битовая проверка: разрешён ли этот рецепт для типа данной станции.
                return;
            
            Container.AddRecipe(new RecipeSlot(recipe, stationType));
        }
    }


    [Serializable]
    public class Recipe
    {
        public List<RecipeSlot> RecipeItems; //хранит все доступные рецепты для конкретной станции

        //[field:SerializeField] public event Action<RecipeSlot> OnRecipeAdded; //Событие, что рецепт добавлен
        
        public void Initialize()
        {
            RecipeItems = new List<RecipeSlot>();
        }

        public void AddRecipe(RecipeSlot recipe) //открывается новый рецепт за баранки
        {
            if(RecipeItems.Contains(recipe))
                return;

            foreach (var existing in RecipeItems)
            {
                if(existing.recipe.Id == recipe.recipe.Id)
                    return;
            }
            
            RecipeItems.Add(recipe);
            //OnRecipeAdded?.Invoke(recipe);
        }
    }

    [Serializable]
    public class RecipeSlot //конкретный рецепт в конкретной ячейке в нашей системе. на какой рецепт
        //кликнул наш пользак
    {
        public CraftingRecipe recipe;
        public CraftingStationType station;
        [HideInInspector]public bool bIsAvailable;

        [field: NonSerialized] public event Action<bool> OnIsAvailableChanged; //событие для UI: перекрасить кнопку рецепта в серый/цветной.

        public RecipeSlot(CraftingRecipe recipe, CraftingStationType station)
        {
            this.recipe = recipe;
            this.station = station;
        }

        public void SetAvailable(bool value)  //можно ли скрафтить этот рецепт прямо сейчас
        {
            if(value == bIsAvailable)
                return;
            bIsAvailable = value;
            OnIsAvailableChanged?.Invoke(value);
        }
    }
}