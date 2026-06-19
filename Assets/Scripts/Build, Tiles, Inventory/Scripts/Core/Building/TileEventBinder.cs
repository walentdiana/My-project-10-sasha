using UnityEngine;              // Unity
using UnityEngine.EventSystems; // нужен для EventTrigger

namespace BuildSystem
{
    // Аналог SlotEventBinder — вешает события на кнопки палитры
    // static — не нужно создавать экземпляр
    // Не содержит логики — только связывает объект с методами PaletteUIManager
    public static class TileEventBinder
    {
        // Вешает три события на кнопку тайла
        public static void BindTileButton(
            GameObject obj,          // GameObject кнопки
            BuildTileData tileData,  // данные тайла
            int paletteId,           // ID палитры
            TilemapLayerType layerType, // тип слоя
            PaletteUIManager ui)     // UI менеджер который получит события
        {
            // _ => — лямбда которая игнорирует параметр (BaseEventData нам не нужен)
            // Навели мышь на кнопку → показать превью этого тайла
            AddEvent(obj, EventTriggerType.PointerEnter, _ => ui.OnEnter(tileData, paletteId, layerType));

            // Убрали мышь с кнопки → вернуть старое превью
            AddEvent(obj, EventTriggerType.PointerExit, _ => ui.OnExit());

            // Кликнули на кнопку → выбрать этот тайл как активный
            AddEvent(obj, EventTriggerType.PointerClick, _ => ui.OnSelect(tileData, paletteId, layerType));
        }

        // Вспомогательный метод: добавляет одно событие
        private static void AddEvent(
            GameObject obj,
            EventTriggerType type,
            UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>(); // берём компонент

            if (!trigger) // если нет — добавляем
                trigger = obj.AddComponent<EventTrigger>();

            // Создаём запись о событии
            var entry = new EventTrigger.Entry { eventID = type };

            entry.callback.AddListener(action); // добавляем обработчик
            trigger.triggers.Add(entry);         // регистрируем
        }
    }
}