using Inventory.Container;    // нужен для InventorySlot
using Inventory.ItemDatabase; // нужен для ItemDatabaseObject
using TMPro;                  // TextMeshPro — красивый текст
using UnityEngine;            // Unity
using UnityEngine.UI;         // нужен для Image

namespace Inventory.Core
{
    // Визуал одной ячейки инвентаря — MonoBehaviour на каждом GameObject слота
    // Показывает иконку предмета и количество
    // Подписывается на изменения ячейки — сам перерисовывается
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;              // Image компонент: иконка предмета
        [SerializeField] private Image amountBackground;     // фон под числом (скрываем если 1 шт)
        [SerializeField] private TextMeshProUGUI amountText; // текст с количеством ("5", "10"...)

        private InventorySlot _slot;          // данные ячейки (откуда читаем)
        private ItemDatabaseObject _database; // база данных (для иконок по ID)

        // Привязывает этот вид к конкретной ячейке данных
        // Вызывается один раз при создании (из CreateSlots)
        public void Bind(InventorySlot slot, ItemDatabaseObject database)
        {
            if (_slot != null)              // если уже были привязаны
                _slot.OnChanged -= Refresh; // отписываемся от старой ячейки

            _slot = slot;         // запоминаем новую ячейку
            _database = database; // запоминаем базу данных

            _slot.OnChanged += Refresh; // подписываемся: ячейка изменилась → Refresh()

            Refresh(); // сразу рисуем текущее состояние
        }

        // Перерисовывает визуал по текущим данным ячейки
        public void Refresh()
        {
            if (_slot.ID >= 0) // ID >= 0 означает что в ячейке есть предмет
            {
                // Получаем иконку из базы данных по ID предмета
                itemIcon.sprite = _database.GetItem[_slot.item.Id].uiDisplay;
                itemIcon.color = Color.white; // белый = полностью видимый (alpha=1)

                if (_slot.amount > 1) // больше одного — показываем количество
                {
                    amountBackground.color = Color.white;      // фон виден
                    amountText.text = _slot.amount.ToString(); // пишем число
                }
                else // один предмет — число не нужно
                {
                    amountBackground.color = Color.clear; // фон прозрачный
                    amountText.text = string.Empty;        // пустая строка
                }
            }
            else // ID == -1, ячейка пустая
            {
                itemIcon.sprite = null;               // убираем иконку
                itemIcon.color = Color.clear;         // делаем прозрачной
                amountBackground.color = Color.clear; // фон прозрачный
                amountText.text = string.Empty;        // пустой текст
            }
        }

        // OnDestroy — Unity вызывает при уничтожении объекта
        // Важно отписаться от события, иначе будет утечка памяти
        private void OnDestroy()
        {
            if (_slot != null)
                _slot.OnChanged -= Refresh; // отписываемся
        }
    }
}