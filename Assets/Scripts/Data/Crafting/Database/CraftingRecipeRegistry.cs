using System.Collections.Generic;
using UnityEngine;

namespace Data.Crafting.Container
{
    [CreateAssetMenu(fileName = "Crafting Recipe Database", menuName = "Crafting System/Database", order = 0)]
    public class CraftingRecipeRegistry : ScriptableObject, ISerializationCallbackReceiver
    {
        public CraftingRecipe[] Recipe;
        private Dictionary<int, CraftingRecipe> _recipes;

        public void OnAfterDeserialize()
        {
            _recipes = new Dictionary<int, CraftingRecipe>();
            if (Recipe == null)
                return;

            for (int i = 0; i < Recipe.Length; i++)
            {
                Recipe[i].Id = i;
                _recipes.Add(i, Recipe[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            _recipes =  new Dictionary<int, CraftingRecipe>();
        }

        public bool TryGetRecipe(int id, out CraftingRecipe recipe)  //поиск рецепта 
        {
            return _recipes.TryGetValue(id, out recipe);
        }
    }
}