using System;
using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Item/Food Object")]
    public class FoodObject : ItemObject
    {
        public void Awake()
        {
            Type = ItemType.Food;
        }
    }
}