using System.Collections.Generic;
using Inventory.Container;
using UnityEngine;
using UnityEngine.UI;

    public abstract class InventoryView : MonoBehaviour
    {
        [SerializeField] Player _player;
        
        [SerializeField] private Vector2 hoverItemSize =  new Vector2(50f, 50f);
        [SerializeField] internal InventoryObject inventory;

        public Dictionary<GameObject, InventorySlot> itemsDisplay = new Dictionary<GameObject, InventorySlot>();


        private void OnEnable()
        {
            inventory.OnChange += RefreshUI;
        }

        private void OnDisable()
        {
            inventory.OnChange -= RefreshUI;
        }

        private void Start()
       {
            CreateSlots();
            SlotEventBinder.BindInventoryEvents(gameObject, this);
            RefreshUI();
       }

        public abstract void CreateSlots();
        public abstract void RefreshUI();
        
        public void OnEnterInterface(GameObject obj)
        {
            _player.mouseItem.ui = obj.GetComponent<InventoryView>();
        }

        public void OnExitInterface(GameObject obj)
        {
            _player.mouseItem.ui = null;
        }

        public void OnEnter(GameObject obj)
        {
            _player.mouseItem.hoverObj = obj;
            
            if(itemsDisplay.ContainsKey(obj))
                _player.mouseItem.hoverItem = itemsDisplay[obj];
        }

        public void OnExit(GameObject obj)
        {
            _player.mouseItem.hoverObj = null;
            _player.mouseItem.hoverItem = null;
        }

        public void OnDragStart(GameObject obj)
        {
            GameObject mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = hoverItemSize;
            mouseObject.transform.SetParent(transform.parent);

            if (itemsDisplay[obj].ID >= 0)
            {
                var image = mouseObject.AddComponent<Image>();
                image.sprite = inventory.database.GetItem[itemsDisplay[obj].ID].uiDisplay;
                image.raycastTarget = false;
            }
            _player.mouseItem.obj = mouseObject;
            _player.mouseItem.item = itemsDisplay[obj];
        }

        public void OnDragEnd(GameObject obj)
        {
            var mouse = _player.mouseItem;
            if (!mouse.ui)
            {
                inventory.RemoveItem(itemsDisplay[obj].item);
                RefreshUI();
            }
            else
            {
                var fromInventory = inventory;
                var toInventory = mouse.ui.inventory;

                var ctx = new InventoryTransferContext
                {
                    FromInventory = fromInventory,
                    ToInventory = toInventory,

                    FromSlot = itemsDisplay[obj],
                    ToSlot = mouse.hoverItem
                };

                fromInventory.Transfer(ctx);
            }

            Destroy(mouse.obj);
            mouse.item = null;
        }

        public void OnDrag(GameObject obj)
        {
            if(_player.mouseItem.obj)
                _player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public class MouseItems
    {
        public InventoryView ui;
        public GameObject obj;
        public InventorySlot item;
        public InventorySlot hoverItem;
        public GameObject hoverObj;
    }
