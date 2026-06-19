using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.Item
{
    public enum ItemType
    {
        Food,
        Equipment,
        Default
    }

    public enum Attributes
    {
        Agility,
        Stamina,
        Strength,
        Intelligence,
        Health,
        Default
    }

    public abstract class ItemObject : ScriptableObject
    {
        public int Id;
        public Sprite uiDisplay;
        public string Name;
        [TextArea(15, 20)]
        public string Description;
        public int MaxStack;
        public ItemCategory Category;
        
        public ItemAttributes[] ItemAttributesConfig;

        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
    }

    [System.Serializable]
    public class Item
    {
        public int Id;
        public string Name;
        public int MaxStack;
        public ItemAttributes[] ItemAttributesConfig;
        [field: SerializeField]public ItemObject Source { get; private set; }

        public Item(ItemObject item)
        {
            Source = item;
            Name = item.Name;
            Id = item.Id;
            MaxStack = item.MaxStack;
            ItemAttributesConfig = new ItemAttributes[item.ItemAttributesConfig.Length];
            for (int i = 0; i < ItemAttributesConfig.Length; i++)
            {
                ItemAttributesConfig[i] = new ItemAttributes(
                    item.ItemAttributesConfig[i].min,
                    item.ItemAttributesConfig[i].max)
                {
                    Attributes = item.ItemAttributesConfig[i].Attributes
                };
            }
        }
    }

    [System.Serializable]
    public class ItemAttributes
    {
        public Attributes Attributes;
        public int value;
        public int min;
        public int max;

        public ItemAttributes(int _min, int _max)
        {
            min = _min;
            max = _max;
            GeneratedValue();
        }

        public void GeneratedValue()
        {
            value = Random.Range(min, max);
        }
    }
}