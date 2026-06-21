using UnityEngine; // Unity

namespace Inventory.Core
{
    // Управляет открытием/закрытием окна полного инвентаря
    // MonoBehaviour — вешается на GameObject на сцене
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryRoot; // корневой объект UI инвентаря

        private bool _isOpen; // открыт ли инвентарь сейчас

        // Открывает инвентарь
        public void Open()
        {
            if (_isOpen) return; // уже открыт — ничего не делаем

            _isOpen = true;                      // запоминаем что открыт
            _inventoryRoot.SetActive(true);      // показываем UI
        }

        // Закрывает инвентарь
        public void Close()
        {
            if (!_isOpen) return; // уже закрыт — ничего не делаем

            _isOpen = false;                     // запоминаем что закрыт
            _inventoryRoot.SetActive(false);     // прячем UI
        }

        // Переключает: открыт — закрыть, закрыт — открыть
        // Можно повесить на кнопку "I" в InputComponent
        public void Toggle()
        {
            if (_isOpen) Close(); // открыт → закрыть
            else Open();          // закрыт → открыть
        }
    }
}