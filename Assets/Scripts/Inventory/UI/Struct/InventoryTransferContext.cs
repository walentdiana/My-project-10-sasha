using Inventory.Container;

    public struct InventoryTransferContext
    {
        public InventoryObject FromInventory;
        public InventoryObject ToInventory;
        
        public InventorySlot FromSlot;
        public InventorySlot ToSlot;
    }
