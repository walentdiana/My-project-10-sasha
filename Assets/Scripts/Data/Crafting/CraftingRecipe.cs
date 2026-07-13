using System;
using Inventory.Item;
using UnityEditor;
using UnityEngine;

namespace Data.Crafting
{
    [Flags]
    public enum CraftingStationType
    {
        None = 0,
        Player = 1 << 0,
        WorkBench = 1 << 1,
        Forge = 1 << 2
    }
    
    
    [Serializable]
    public class CraftingIngredients
    {
        public ItemObject Item;
        public int Amount;
        
    }
    
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting System/Recipe", order = 0)]
    public class CraftingRecipe : ScriptableObject
    {
        public int Id;
        public Sprite Icon;
        [TextArea] public string Discription;
        
        [Header("Crafting Station")]
        public CraftingStationType CraftingStationType;
        
        [Header("Crafting Result")]
        public ItemObject ItemResult;
        public int ResultAmount;
        
        [Header("Crafting Ingredients")]
        public CraftingIngredients[] ItemIngredients;
        
        
    }
    
}