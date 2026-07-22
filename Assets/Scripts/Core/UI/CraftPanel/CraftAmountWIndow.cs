using System.Collections.Generic;
using Core.Crafting;
using Core.Crafting.UI;
using Data.Crafting.Container;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.CraftPanel
{
    public class CraftAmountWIndow : MonoBehaviour
    {
        [SerializeField] internal GameObject _craftPanel;
        [SerializeField] private Slider _slider;
        [SerializeField] private Transform _ingredientsContainer;
        [SerializeField] private GameObject _ingredientPrefab;
        [SerializeField] private Image _recipeImage;
        [SerializeField] private Button _craftConfirmButton;
        [SerializeField] private TMP_Text _resultAmountText;

        private CraftSession _session;
        private RecipeSlot _slot;
        private readonly List<IngredientRowView> _rows = new();
        
        
        private void Awake()
        {
            _craftPanel.SetActive(false);
        }

        public void Open(RecipeSlot slot,CraftSession session)
        {
            if (_slot != null && _slot != slot)
            {
                _session.RecalculateCurrentRecipe(_slot, 1);
            }
            _slot = slot;
            _session = session;
            _recipeImage.sprite = _slot.recipe.Icon;
            _craftConfirmButton.onClick.RemoveAllListeners();
            
            BuildRaws();
            
            _slider.onValueChanged.RemoveAllListeners();
            _slider.minValue = 1;
            
            _slider.value = 1;
            _slider.onValueChanged.AddListener(OnSliderChanged);
            
            _craftPanel.SetActive(true);
            OnSliderChanged(_slider.value);
            _craftConfirmButton.onClick.AddListener(Confirm);
        }

        private void BuildRaws()
        {
            ClearRaws();

            foreach (var ingredients in _slot.recipe.ItemIngredients)
            {
                var obj = Instantiate(_ingredientPrefab, _ingredientsContainer);
                _rows.Add(obj.GetComponent<IngredientRowView>());
            }
        }

        private void OnSliderChanged(float value)
        {
            int count = Mathf.RoundToInt(value);

            for (int i = 0; i < _slot.recipe.ItemIngredients.Length; i++)
            {
                var ingredient = _slot.recipe.ItemIngredients[i];
                int required = ingredient.Amount * count;
                int available = _session.GetItemCount(ingredient.Item.Id);

                _rows[i].Refresh(ingredient.Item.uiDisplay, required, available);
            }
            
            int resultAmount = _slot.recipe.ResultAmount * count;
            _resultAmountText.text = $"{resultAmount}";
        }

        private void Confirm()
        {
            _session.Craft(_slot, Mathf.RoundToInt(_slider.value));
            Close();
        }

        private void ClearRaws()
        {
            foreach (var row in _rows)
                Destroy(row.gameObject);
            _rows.Clear();
        }

        public void Close()
        {
            ClearRaws();
            if(_slot != null)
                _session.RecalculateCurrentRecipe(_slot, 1);
            
            _slider.onValueChanged.RemoveListener(OnSliderChanged);
            _craftPanel.SetActive(false);
            
            _slot = null;
            _session = null;
        }
    }
}