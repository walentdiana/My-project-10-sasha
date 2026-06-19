using System;
using Inventory.ItemDatabase;
using Inventory.Item;
using UnityEngine;

namespace Inventory.Container
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public ItemDatabaseObject database;
        public Inventory Container;
        
        public event Action OnChange;

        public void AddItem(Item.Item _item, int _amount)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].ID == _item.Id)
                {
                    Container.Items[i].AddAmount(_amount);
                    return;
                }
            }
            SetEmptySlot(_item, _amount);
        }
        public void Transfer(InventoryTransferContext ctx)
        {
            var from = ctx.FromSlot;
            var to = ctx.ToSlot;
            
            if (from.item.Id == to.item.Id && to.amount <= 98)
            {
                to.AddAmount(from.amount);
                RemoveItem(from.item);
            }
            else
            {
                InventorySlot temp = new InventorySlot(to.ID, to.item, to.amount);
                to.UpdateSlot(from.ID, from.item, from.amount);
                from.UpdateSlot(temp.ID, temp.item, temp.amount);
            }

            ctx.FromInventory.OnChange?.Invoke();
            ctx.ToInventory.OnChange?.Invoke();
        }

        public void RemoveItem(Item.Item _item)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item == _item)
                {
                    Container.Items[i].UpdateSlot(-1, null, 0);
                }
            }
        }

        private void SetEmptySlot(Item.Item item, int amount)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].ID <= -1)
                {
                    Container.Items[i].UpdateSlot(item.Id, item, amount);
                }
            }
        }
    }

    [Serializable]
    public class Inventory
    {
        public InventorySlot[] Items = new InventorySlot[20];
    }

    [Serializable]
    public class InventorySlot
    {
        public InventoryView parent;
        public ItemType[] AllowedItems = new ItemType[0];
        
        public event Action OnChange;
        
        public int ID = -1;
        public Item.Item item;
        public int amount;

        public InventorySlot()
        {
            ID = -1;
            item = null;
            amount = 0;
        }

        public InventorySlot(int id, Item.Item item, int amount)
        {
            ID = id;
            this.item = item;
            this.amount = amount;
        }

        public void UpdateSlot(int id, Item.Item item, int amount)
        {
            ID = id;
            this.item = item;
            this.amount = amount;
            OnChange?.Invoke();
        }

        public void AddAmount(int value)
        {
            amount += value;
            OnChange?.Invoke();
        }

        public bool CanPlaceInSlot(ItemObject _item)
        {
            if (AllowedItems.Length <= 0)
                return true;

            for (int i = 0; i < AllowedItems.Length; i++)
            {
                if(_item.Type == AllowedItems[i])
                    return true;
            }
            return false;
        }
    }
}