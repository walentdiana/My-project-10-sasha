using UnityEngine;

namespace Core.Crafting.UI
{
    public class RecipeViewStation : RecipeView
    {
        public override void CreateSlots()
        {
            _slots = Station.Container.RecipeItems;

            foreach (var slot in _slots)
            {
                var obj = Instantiate(SlotPrefab, transform);
                RecipeEventBinder.BindSlotEvent(obj, slot, this);

                var view = obj.GetComponent<SlotView>();
                view.Bind(slot, database);
            }
            
        }
    }
}