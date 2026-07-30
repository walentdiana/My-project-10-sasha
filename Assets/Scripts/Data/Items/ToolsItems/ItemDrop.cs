using UnityEngine;


namespace Inventory.Item
{
    public class ItemDrop
    {
        public ItemObject Item;
        public int MinAmount;
        public int MaxAmount;


        public int RollAmount() => Random.Range(MinAmount, MaxAmount + 1);

    }
}