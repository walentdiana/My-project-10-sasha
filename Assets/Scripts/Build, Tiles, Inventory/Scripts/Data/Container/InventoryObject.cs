using System;
using Inventory.Item;
using Inventory.ItemDatabase;
using UnityEngine;

namespace Inventory.Container
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public ItemDatabaseObject database;
        public Inventory Container;
        
        public event Action OnChanged;
        
        public void AddItem(Item.Item _item, int _amount)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].ID == _item.Id)
                {
                    if (Container.Items[i].amount < Container.Items[i].item.MaxStack)
                    {
                        Container.Items[i].AddAmount(_amount);
                        return;
                    }
                }
            }
            SetEmptySlot(_item, _amount);
        }

        public void MoveItem(InventorySlot item1, InventorySlot item2)
        {
            if (item1.item.Id == item2.item.Id)
            {
                int amount = item1.amount + item2.amount;
                
                if (item2.amount < amount)
                {
                    item2.UpdateSlot(item2.ID, item2.item, amount);
                    RemoveItem(item1.item);
                    return;
                }
            }

            InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
            item2.UpdateSlot(item1.ID, item1.item, item1.amount);
            item1.UpdateSlot(temp.ID, temp.item, temp.amount);
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

        [ContextMenu("Clear")]
        public void Clean()
        {
            foreach (var sell in Container.Items)
            {
                RemoveItem(sell.item);
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
       [field: NonSerialized] public event Action OnChanged;
        
        public ItemType[] AllowedItems = new ItemType[0];
        
        public ItemCategory AllowedCategories =  ItemCategory.None;
        
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
            OnChanged?.Invoke();
        }

        public void AddAmount(int value)
        {
            amount += value;
            OnChanged?.Invoke();
        }
        
        //-----------------------------------------------------------
        public bool CanPlaceInSlot(ItemObject _item)
        {
            if (AllowedCategories == ItemCategory.None)
                return true;
            return (AllowedCategories & _item.Category) != 0;
        }
    }
    
    public static class InventorySlotExtensions
    {
        public static bool TryConsume(this InventorySlot slot)
        {
            if (slot.amount <= 1)
            {
                slot.UpdateSlot(-1, null, 0);
                return false; // предмет закончился
            }
            slot.AddAmount(-1);
            return true; // ещё остались
        }
    }
}