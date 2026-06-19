using UnityEngine;
using UnityEngine.EventSystems;

namespace BuildSystem
{
    // Аналог SlotEventBinder из инвентаря.
    // Статический хелпер — вешает EventTrigger события на GameObject кнопки.
    // Не знает ничего о логике — только связывает объект с методами PaletteUIManager.
    public static class TileEventBinder
    {
        public static void BindTileButton(
            GameObject obj,
            BuildTileData tileData,
            int paletteId,
            TilemapLayerType layerType,
            PaletteUIManager ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, _ => ui.OnEnter(tileData, paletteId, layerType));
            AddEvent(obj, EventTriggerType.PointerExit,  _ => ui.OnExit());
            AddEvent(obj, EventTriggerType.PointerClick, _ => ui.OnSelect(tileData, paletteId, layerType));
        }

        private static void AddEvent(
            GameObject obj,
            EventTriggerType type,
            UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if (!trigger)
                trigger = obj.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
    }
}
