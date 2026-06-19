using UnityEngine;

namespace Inventory.Core
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryRoot;
        //[SerializeField] private InputComponent _input;

        private bool _isOpen;

        /*private void Update()
        {
            if (_input.InventoryPressed())
            {
                if (_isOpen) Close();
                else Open();
            }
        }*/

        public void Open()
        {
            if (_isOpen) return;

            _isOpen = true;
            _inventoryRoot.SetActive(true);
        }

        public void Close()
        {
            if (!_isOpen) return;

            _isOpen = false;
            _inventoryRoot.SetActive(false);
        }

        public void Toggle()
        {
            if (_isOpen) Close();
            else Open();
        }
    }
}