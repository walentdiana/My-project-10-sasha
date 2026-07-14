using Data.Crafting;
using Data.Crafting.Container;
using Inventory.Container;

namespace Core.Crafting
{
    public class CraftSession
    {
        private readonly CraftingStation _station;
        private readonly  InventoryObject _inventory;

        public CraftSession(CraftingStation station, InventoryObject inventory)  //если класс не монобез и не so, то передать параметры моем только через конструктор 
        {
            _station = station;
            _inventory = inventory;
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }

        public bool CanCraft(RecipeSlot slot)
        {
            foreach (var ingredient in slot.recipe.ItemIngredients)
                if(!IsSatisfied(ingredient))
                    return false;
            
            return true;
        }

        private bool IsSatisfied(CraftingIngredients ingredient)
        {
            int total = 0;

            foreach (var invSlot in _inventory.Container.Items)
            {
                if (invSlot.ID >= 0 && invSlot.item.Id == ingredient.Item.Id)
                    total += invSlot.amount;
            }

            return total >= ingredient.Amount;
        }
    }
}