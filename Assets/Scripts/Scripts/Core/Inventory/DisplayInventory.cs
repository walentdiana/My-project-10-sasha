using System.Collections.Generic;
using Inventory.Container;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class DisplayInventory : MonoBehaviour
    {
        /*public GameObject cellPrefab;
        
        
        private Dictionary<GameObject, InventorySlot> itemsDisplay = new Dictionary<GameObject, InventorySlot>();
        public MouseItem mouseItem = new MouseItem();
        
        private void Start()
        {
            CreateSlots();
        }

        private void UpdateSlots()
        {
            foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplay)
            {
                if (slot.Value.ID >= 0)
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        inventory.database.GetItem[slot.Value.item.Id].uiDisplay;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color =
                        new Color(1, 1, 1, 1);

                    if (slot.Value.amount > 1)
                    {
                        slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().color =
                            new Color(1, 1, 1, 1);
                        slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount.ToString();
                    }
                    else
                    {
                        slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().color =
                            new Color(0, 0, 0, 0);
                        slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }

                }
                else
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        null;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color =
                        new Color(0, 0, 0, 0);
                    slot.Key.transform.GetChild(1).GetComponentInChildren<Image>().color =
                        new Color(0, 0, 0, 0);
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }
        }

        private void CreateSlots()
        {
            itemsDisplay = new Dictionary<GameObject, InventorySlot>();
            
            for (int i = 0; i < inventory.Container.Items.Length; i++)
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit,  delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.BeginDrag,    delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.EndDrag,      delegate { OnDragEnd(obj); });
                AddEvent(obj, EventTriggerType.Drag,         delegate { OnDrag(obj); });
                
                itemsDisplay.Add(obj, inventory.Container.Items[i]);
            }

            UpdateSlots();
        }

        private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            var trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        private void OnEnter(GameObject obj)
        {
            mouseItem.hoverObj = obj;
            if(itemsDisplay.ContainsKey(obj))
                mouseItem.hoverItem = itemsDisplay[obj];
        }

        private void OnExit(GameObject obj)
        {
            mouseItem.hoverObj = null;
            mouseItem.hoverItem = null;
        }

        private void OnDragStart(GameObject obj)
        {
            GameObject mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = hoverItemSize;
            rt.localScale = Vector3.one;
            mouseObject.transform.SetParent(transform);

            if (itemsDisplay[obj].ID >= 0)
            {
                var image = mouseObject.AddComponent<Image>();
                image.sprite = inventory.database.GetItem[itemsDisplay[obj].ID].uiDisplay;
                image.raycastTarget = false;
            }
            mouseItem.obj = mouseObject;
            mouseItem.item = itemsDisplay[obj];
        }

        private void OnDragEnd(GameObject obj)
        {
            if (mouseItem.hoverObj)
            {
                inventory.MoveItem(itemsDisplay[obj], itemsDisplay[mouseItem.hoverObj]);
            }
            else
            {
                inventory.RemoveItem(itemsDisplay[obj].item);
            }
            
            UpdateSlots();
            
            Destroy(mouseItem.obj);
            mouseItem.item =  null;
        }

        private void OnDrag(GameObject obj)
        {
            if(mouseItem.obj)
                mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }

        public Vector3 GetPosition(int index)
        {
            return new Vector3(
                x_Start + (x_Space_Between_Item * (index % number_Of_Column)),
                y_Start + (-y_Space_Between_Item * (index / number_Of_Column)),
                0f
            );
        }*/
    }

    /*public class MouseItem
    {
        public GameObject obj;
        public InventorySlot item;
        public InventorySlot hoverItem;
        public GameObject hoverObj;
    }*/
}