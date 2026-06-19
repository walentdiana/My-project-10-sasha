using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BuildSystem
{
    public static class TileEventBinder
    {
        public static void BindTileEvent(
            GameObject obj, 
            BuildTileData tileData,
            int paletteId,
            TilemapLayerType layerType,
            PaletteUIManager uiManager)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, _ => uiManager.OnEnter(tileData, paletteId, layerType));
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { uiManager.OnExit(); });
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { uiManager.OnSelect(tileData, paletteId, layerType); });
           
        }
        
        private static void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            var trigger = obj.GetComponent<EventTrigger>();
            
            if(!trigger)
                trigger = obj.AddComponent<EventTrigger>();
            
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
    }
}