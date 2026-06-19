using UnityEngine;

    public class StaticInventoryView : InventoryView
    {
        public GameObject[] slots;

        private InventorySlotRenderer _renderer;
        private ISlotSource _source;
        
        public override void CreateSlots()
        {
           _renderer =  new InventorySlotRenderer(inventory);
           _source = new StaticSlotSource(slots);

           itemsDisplay = _renderer.CreateSlots(
                _source,
                null,
                transform,
                this);
        }

        public override void RefreshUI()
        {
            _renderer.UpdateSlots();
        }
    }
