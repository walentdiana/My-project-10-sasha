using Inventory.Container;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory.Core
{
    public static class SlotEventBinder
    {
        public static void BindSlotEvent(GameObject obj, InventorySlot slot, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnter(slot); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { ui.OnExit(slot); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { ui.OnDragStart(slot); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { ui.OnDragEnd(slot); });
            AddEvent(obj, EventTriggerType.Drag, delegate { ui.OnDrag(slot); });
        }

        public static void BindInventoryEvent(GameObject obj, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnterInterface(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { ui.OnExitInterface(obj); });
        }

        public static void BindClickEvent(GameObject obj, InventorySlot slot, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerClick, delegate { ui.OnSlotClick(slot); });
        }

        private static void AddEvent(
            GameObject obj,
            EventTriggerType type,
            UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();

            var entry = new EventTrigger.Entry
            {
                eventID = type
            };

            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
    }
}
