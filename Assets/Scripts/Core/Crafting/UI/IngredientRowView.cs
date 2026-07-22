using System;
using Data.Crafting.Container;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Crafting.UI
{
    public class IngredientRowView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countLabel;
        [SerializeField] private TextMeshProUGUI _itemCountLabel;
        
        private static readonly Color ENOUGH_COLOR = Color.black;
        private static readonly Color NOT_ENOUGH_COLOR = Color.red;
        private static readonly Color AVAILABLE_COLOR = Color.white;
        private static readonly Color NOT_AVAILABLE_COLOR = new Color(1f,1f,1f,0.3f);


        public void Refresh(Sprite sprite, int required, int available)
        {
            _icon.sprite = sprite;
            _countLabel.text = $"{required}";
            _itemCountLabel.text = $"{available}";
            _countLabel.color = available >= required ? ENOUGH_COLOR  : NOT_ENOUGH_COLOR;
            _icon.color = available >= required ? AVAILABLE_COLOR : NOT_AVAILABLE_COLOR;
        }
        
    }
}