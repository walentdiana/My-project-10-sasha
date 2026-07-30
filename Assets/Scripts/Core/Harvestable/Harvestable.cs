using Core.TimeSystem;
using Data.Harvestable;
using Inventory.Container;
using Inventory.Item;
using Inventory.ItemDatabase;
using UnityEngine;
using Zenject;


namespace Core.Harvestable
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Harvestable : MonoBehaviour
    {
        [SerializeField] private ItemDatabaseObject _databaseObject;
        [SerializeField] private HarvestableData _data;
        [SerializeField] private int _entityId;


        [SerializeField] private float _shakeStrength = 0.1f;
        [SerializeField] private float _shakeDuration = 0.1f;


        private int _currentHP;
        private bool _bIsDead;


        private InventoryObject _inventory;
        private HarvestableRespawnSystem _respawnSystem;
        private ITimeService _timeService;


        public int EntityId => _entityId;
        public bool bIsDead => _bIsDead;
        
        [Inject]
        public void Construct(
            InventoryObject inventory,
            HarvestableRespawnSystem respawnSystem,
            ITimeService timeService)
        {
            _inventory = inventory;
            _respawnSystem = respawnSystem;
            _timeService = timeService;
        }

        private void Awake() => _respawnSystem.Register(this);

        private void OnDestroy() => _respawnSystem.Unregister(_entityId);

        private void Start() => _currentHP = _data.MaxHP;
        
        public void Interact(ToolItemObject tool = null)
        {
            if(_bIsDead) return;
            if(!CanHarvestWith(tool)) return;
        }


        private bool CanHarvestWith(ToolItemObject tool)
        {
            if (!tool) return _data.IsAllowsHandHarvest;


            return (tool.Capabilities & _data.RequiredCapability) != 0;
        }
    }
}