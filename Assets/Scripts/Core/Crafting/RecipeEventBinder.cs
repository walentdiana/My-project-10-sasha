using Core.Crafting.UI;
using Data.Crafting.Container;
using Inventory.Container;
using Inventory.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Crafting
{
    public class RecipeEventBinder
    {
         // Вешает hover и drag-and-drop события на один слот
        public static void BindSlotEvent(GameObject obj, RecipeSlot slot, RecipeView ui)
        {
            // delegate { ... } — анонимная функция (маленький кусок кода без имени)
            // Когда мышь войдёт в obj — вызовется ui.OnEnter(slot)
           // AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnter(slot); });
           // AddEvent(obj, EventTriggerType.PointerExit,  delegate { ui.OnExit(slot); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { ui.OnSlotClicked(slot); });
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