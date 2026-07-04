using Inventory.Container; // нужен для InventorySlot, InventoryObject
using UnityEngine;          // Unity
using UnityEngine.UI;       // нужен для Image (иконки предметов)

namespace Inventory.Core
{
    // Базовый класс для любого UI инвентаря.
    // Содержит логику drag-and-drop. Сам по себе не используется — только через наследников.
    public abstract class InventoryView : MonoBehaviour
    {
        public InventoryObject inventory; // ScriptableObject с данными инвентаря
        public GameObject slotPrefab;     // префаб одной ячейки UI (назначается в инспекторе)

        // static readonly — один объект на весь класс (не на каждый экземпляр)
        // Хранит состояние мыши при перетаскивании предмета
        protected static readonly MouseItems mouseItem = new MouseItems();

        protected InventorySlot[] slots; // массив ячеек данных из инвентаря

        // static — один объект для всего приложения
        // Картинка предмета которую тащим за курсором
        private static GameObject _dragVisual;

        // virtual — наследники МОГУТ переопределить. По умолчанию ничего не делает.
        public virtual void OnSlotClick(InventorySlot slot) { }

        // abstract — наследники ОБЯЗАНЫ реализовать
        // Создаёт все GameObject-ы ячеек на сцене
        public abstract void CreateSlots();

        // Start — Unity вызывает при появлении объекта на сцене
        void Start()
        {
            CreateSlots(); // создаём ячейки (реализация в наследнике)
            SlotEventBinder.BindInventoryEvent(gameObject, this); // вешаем события на весь инвентарь
            _dragVisual = GetDragVisual(); // создаём или получаем объект-иконку для drag
        }

        // Вызывается когда курсор входит в зону ячейки
        public void OnEnter(InventorySlot slot)
        {
            mouseItem.toSlot = slot; // запоминаем: вот куда можно бросить предмет
        }

        // Вызывается когда курсор уходит из зоны ячейки
        public void OnExit(InventorySlot slot)
        {
            mouseItem.toSlot = null; // больше нет целевой ячейки
        }

        // Курсор вошёл в зону ВСЕГО инвентаря (не конкретной ячейки)
        public void OnEnterInterface(GameObject obj)
        {
            mouseItem.ui = this; // этот инвентарь — текущий получатель drop
        }

        // Курсор ушёл из зоны всего инвентаря
        public void OnExitInterface(GameObject obj)
        {
            mouseItem.ui = null; // нет получателя
        }

        // Начало перетаскивания — показываем иконку предмета под курсором
        public void OnDragStart(InventorySlot slot)
        {
            if (slot.ID >= 0) // если в ячейке что-то есть
            {
                var img = _dragVisual.GetComponent<Image>(); // берём Image компонент
                img.sprite = inventory.database.GetItem[slot.ID].uiDisplay; // ставим иконку
                img.raycastTarget = false; // иконка не блокирует клики мыши
                _dragVisual.SetActive(true); // показываем иконку
            }

            mouseItem.obj = _dragVisual; // запоминаем объект иконки
            mouseItem.item = slot;        // запоминаем какую ячейку тащим
        }

        // Конец перетаскивания — кладём предмет или выбрасываем
        public void OnDragEnd(InventorySlot fromSlot)
        {
            if (!mouseItem.ui) // курсор не над инвентарём — выбросить предмет
            {
                if (mouseItem.toSlot != null)
                    inventory.RemoveItem(mouseItem.toSlot.item);
            }
            else // курсор над инвентарём — переложить предмет
            {
                inventory.MoveItem(mouseItem.toSlot, fromSlot);
            }

            _dragVisual.SetActive(false); // прячем иконку
            mouseItem.item = null;         // сбрасываем перетаскиваемое
        }

        // Во время перетаскивания — двигаем иконку за курсором каждый кадр
        public void OnDrag(InventorySlot slot)
        {
            if (mouseItem.obj) // если есть объект иконки
            {
                // Перемещаем иконку на позицию курсора
                mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
            }
        }

        // Создаёт объект-иконку для drag, или возвращает существующий
        private GameObject GetDragVisual()
        {
            if (!_dragVisual) // если ещё не создан
            {
                _dragVisual = new GameObject("DragVisual"); // новый пустой GameObject
                var rt = _dragVisual.AddComponent<RectTransform>(); // добавляем UI transform
                rt.sizeDelta = new Vector2(100, 100);               // размер 100x100 пикселей
                _dragVisual.AddComponent<Image>().raycastTarget = false; // не блокирует клики
            }

            _dragVisual.transform.SetParent(transform.parent); // кладём рядом в иерархии
            _dragVisual.SetActive(false); // по умолчанию скрыт
            return _dragVisual;
        }
    }

    // Состояние мыши при работе с инвентарём
    // Один экземпляр на всё приложение (static в InventoryView)
    public class MouseItems
    {
        public InventoryView ui;      // инвентарь под курсором (null = не над инвентарём)
        public GameObject obj;        // GameObject иконки при drag
        public InventorySlot item;    // ячейка которую тащим
        public InventorySlot toSlot;  // ячейка над которой сейчас курсор (цель drop)
        public GameObject hoverObj;   // объект под курсором (не используется активно)

        // Сбрасывает всё состояние
        public void Clear()
        {
            toSlot = null;   // нет целевой ячейки
            hoverObj = null; // нет объекта под курсором
            item = null;     // нет перетаскиваемого предмета
            ui = null;       // нет активного инвентаря

            if (obj)
                obj.SetActive(false); // прячем иконку drag
        }
    }
}