using Data.Crafting.Container;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Crafting.UI
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private Image _recipeImage;
        
        private RecipeSlot _slot;
        private CraftingRecipeRegistry _database;

        public void Bind(RecipeSlot slot, CraftingRecipeRegistry database)
        {
            _slot = slot;
            _database = database;
            slot.OnIsAvailableChanged += OnAvailabilityChange;
            Refresh();
        }

        private void OnAvailabilityChange(bool obj)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_slot != null && _database.TryGetRecipe(_slot.recipe.Id, out var recipe))
            {
                _recipeImage.sprite = _slot.recipe.Icon;
                _recipeImage.color = _slot.bIsAvailable
                    ? Color.white
                    : new Color(1, 1, 1, 0.35f);
            }
        }

        private void OnDestroy()
        {
            if (_slot != null)
                _slot.OnIsAvailableChanged -= OnAvailabilityChange;
        }
    }
    
}