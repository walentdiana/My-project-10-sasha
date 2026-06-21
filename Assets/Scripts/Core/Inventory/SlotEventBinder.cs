using Inventory.Container;  // нужен для InventorySlot
using UnityEngine;           // Unity
using UnityEngine.EventSystems; // нужен для EventTrigger

namespace Inventory.Core
{
    // static — не нужно создавать экземпляр, вызываем как SlotEventBinder.BindSlotEvent(...)
    // Хелпер: вешает UI события на GameObject ячейки
    // Сам не содержит логики — только связывает объект с методами InventoryView
    public static class SlotEventBinder
    {
        // Вешает hover и drag-and-drop события на один слот
        public static void BindSlotEvent(GameObject obj, InventorySlot slot, InventoryView ui)
        {
            // delegate { ... } — анонимная функция (маленький кусок кода без имени)
            // Когда мышь войдёт в obj — вызовется ui.OnEnter(slot)
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnter(slot); });
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { ui.OnExit(slot); });
            AddEvent(obj, EventTriggerType.BeginDrag,    delegate { ui.OnDragStart(slot); });
            AddEvent(obj, EventTriggerType.EndDrag,      delegate { ui.OnDragEnd(slot); });
            AddEvent(obj, EventTriggerType.Drag,         delegate { ui.OnDrag(slot); });
        }

        // Вешает события на весь инвентарь (не на конкретный слот)
        // Нужно чтобы знать "мышь над инвентарём или нет" при drop
        public static void BindInventoryEvent(GameObject obj, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnterInterface(obj); });
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { ui.OnExitInterface(obj); });
        }

        // Вешает событие клика — для выбора предмета (инструмент, строительство)
        public static void BindClickEvent(GameObject obj, InventorySlot slot, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerClick, delegate { ui.OnSlotClick(slot); });
        }

        // Вспомогательный метод: добавляет одно конкретное событие к EventTrigger
        private static void AddEvent(
            GameObject obj,
            EventTriggerType type,            // тип события (клик, hover, drag...)
            UnityEngine.Events.UnityAction<BaseEventData> action) // функция-обработчик
        {
            // Получаем компонент EventTrigger с объекта (должен быть добавлен заранее)
            EventTrigger trigger = obj.GetComponent<EventTrigger>();

            // Создаём новую запись о событии
            var entry = new EventTrigger.Entry
            {
                eventID = type // указываем тип события
            };

            entry.callback.AddListener(action); // добавляем нашу функцию как слушателя
            trigger.triggers.Add(entry);         // регистрируем в компоненте EventTrigger
        }
    }
}