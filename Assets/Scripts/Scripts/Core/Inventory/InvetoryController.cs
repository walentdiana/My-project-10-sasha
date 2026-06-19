using GameName.Input;
using UnityEngine;

namespace Inventory
{
    public class InvetoryController : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryRoot;
        [SerializeField] private InputComponent _input;

        private bool _bIsOpen;

        private void Update()
        {
            if (_input.InventoryMode())
            {
                if (_bIsOpen)
                    Close();
                else
                    Open();
            }
        }

        private void Close()
        {
            if(!_bIsOpen)
                return;
            _bIsOpen = false;
            _inventoryRoot.SetActive(false);
        }

        public void Open()
        {
            if(_bIsOpen) 
                return;
            _bIsOpen = true;
            _inventoryRoot.SetActive(true);
        }
    }
}