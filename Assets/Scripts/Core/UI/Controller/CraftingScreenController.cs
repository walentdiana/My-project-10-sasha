using System;
using Core.Crafting;
using Core.Crafting.UI;
using Core.UI.CraftPanel;
using Data.Crafting.Container;
using UnityEngine;

namespace Core.UI.Controller
{
    public class CraftingScreenController : MonoBehaviour
    {
        [SerializeField] private RecipeView _recipeView;
        [SerializeField] private  CraftAmountWIndow _craftAmountWIndow;

        private void OnEnable()
        {
            _recipeView.OnRecipeSelected += HandleRecipeSelected;
        }

        private void OnDisable()
        {
            _recipeView.OnRecipeSelected -= HandleRecipeSelected;
        }

        private void HandleRecipeSelected(RecipeSlot slot, CraftSession session)
        {
            if (!_craftAmountWIndow._craftPanel.activeInHierarchy)
            {
                _craftAmountWIndow.Open(slot, session);
                return;
            }
            _craftAmountWIndow.Close();
        }
    }
}