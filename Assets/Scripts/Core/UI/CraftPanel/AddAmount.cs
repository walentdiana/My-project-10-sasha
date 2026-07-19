using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.CraftPanel
{
    public class AddAmount : MonoBehaviour
    {
        [SerializeField]private Button _addButton;
        [SerializeField]private Button _removeButton;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _amountText;

        public event Action OnChangeItemValue;


        private void Awake()
        {
            /*_slider = GetComponentInChildren<Slider>();
            _amountText = GetComponentInChildren<TMP_Text>();
            _addButton = transform.GetChild(2).GetComponentInChildren<Button>();
            _removeButton = transform.GetChild(3).GetComponentInChildren<Button>();*/

            _amountText.text = "0";
        }

        private void OnEnable()
        {
            _addButton.onClick.AddListener(IncreaseAmount);
            _removeButton.onClick.AddListener(DecreaseAmount);
            _slider.onValueChanged.AddListener(OnChangeSliderValue);
        }

        private void OnDisable()
        {
            _addButton.onClick.RemoveAllListeners();
            _removeButton.onClick.RemoveAllListeners();
            _slider.onValueChanged.RemoveAllListeners();
        }

        private void IncreaseAmount()
        {
            _slider.value++;
        }

        private void DecreaseAmount()
        {
            _slider.value--;
        }

        private void OnChangeSliderValue(float arg0)
        {
            _amountText.text = "" + _slider.value;
            OnChangeItemValue?.Invoke();
        }
    }
}