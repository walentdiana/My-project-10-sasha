using Inventory.Container;
using UnityEngine;
using UnityEngine.UI;


namespace Inventory.Core
{
    public abstract class InventoryView : MonoBehaviour
    {
        public InventoryObject inventory;
        public GameObject slotPrefab;
        
        protected static readonly MouseItems mouseItem = new MouseItems();
        protected InventorySlot[] slots;
        private static GameObject _dragVisual;
        
        public virtual void OnSlotClick(InventorySlot slot) { }

        public abstract void CreateSlots();

        void Start()
        {
            CreateSlots();
            SlotEventBinder.BindInventoryEvent(gameObject, this);
            _dragVisual = GetDragVisual();
        }

        public void OnEnter(InventorySlot slot)
        {
            mouseItem.toSlot = slot;
        }

        public void OnExit(InventorySlot slot)
        {
            mouseItem.toSlot = null;
        }

        public void OnEnterInterface(GameObject obj)
        {
            mouseItem.ui = this;
        }

        public void OnExitInterface(GameObject obj)
        {
            mouseItem.ui = null;
        }

        public void OnDragStart(InventorySlot slot)
        {
            if (slot.ID >= 0)
            {
                var img = _dragVisual.GetComponent<Image>();
                img.sprite = inventory.database.GetItem[slot.ID].uiDisplay;
                img.raycastTarget = false;
                _dragVisual.SetActive(true);
            }

            mouseItem.obj = _dragVisual;
            mouseItem.item = slot;
        }

        public void OnDragEnd(InventorySlot fromSlot)
        {
            if (!mouseItem.ui)
            {
                if (mouseItem.toSlot != null)
                    inventory.RemoveItem(mouseItem.toSlot.item);
            }
            else
            {
                inventory.MoveItem(mouseItem.toSlot, fromSlot);
            }

            _dragVisual.SetActive(false);
            mouseItem.item = null;
        }

        public void OnDrag(InventorySlot slot)
        {
            if (mouseItem.obj)
            {
                mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
            }
        }

        private GameObject GetDragVisual()
        {
            if (!_dragVisual)
            {
                _dragVisual = new GameObject("DragVisual");
                var rt = _dragVisual.AddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(100, 100);
                _dragVisual.AddComponent<Image>().raycastTarget = false;
            }

            _dragVisual.transform.SetParent(transform.parent);
            _dragVisual.SetActive(false);
            return _dragVisual;
        }
    }

    public class MouseItems
    {
        public InventoryView ui;
        public GameObject obj;
        public InventorySlot item;
        public InventorySlot toSlot;
        public GameObject hoverObj;

        public void Clear()
        {
            toSlot = null;
            hoverObj = null;
            item = null;
            ui = null;

            if (obj)
                obj.SetActive(false);
        }
    }
}