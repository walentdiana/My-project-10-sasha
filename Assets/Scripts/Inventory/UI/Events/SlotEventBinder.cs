using Inventory.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class SlotEventBinder
    {
        public static void BindSlotEvents(GameObject obj, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { ui.OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag,    delegate { ui.OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag,      delegate { ui.OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag,         delegate { ui.OnDrag(obj); });
        }

        public static void BindInventoryEvents(GameObject obj, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnterInterface(obj); });
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { ui.OnExitInterface(obj); });
        }

        private static void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            var trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }
    }
