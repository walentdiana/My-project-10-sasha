using Core.TimeSystem;
using Data.Harvestable;
using Inventory.Container;
using Inventory.Item;
using Inventory.ItemDatabase;
using UnityEngine;
using Zenject;


namespace Core.Harvestable
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Harvestable : MonoBehaviour
    {
        private const string REQUIRE_LAYER_NAME = "Harvestable";
        
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
        
        private void Start()
        {
            _currentHP = _data.MaxHP;
            EnsureCorrectLayer();
        }

        public void Interact(ToolItemObject tool = null)
        {
            if(!CanHarvestWith(tool)) return;
        }


        private bool CanHarvestWith(ToolItemObject tool)
        {
            if (!tool) return _data.IsAllowsHandHarvest;


            return (tool.Capabilities & _data.RequiredCapability) != 0;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EnsureCorrectLayer();
        }
        
#endif
        private void EnsureCorrectLayer()
        {
            int requiredLayer = LayerMask.NameToLayer(REQUIRE_LAYER_NAME);

            if (requiredLayer < 0)
            {
                Debug.Log($"[Harvestable] Layer '{REQUIRE_LAYER_NAME}' Не существует.]");
                return;
            }
            
            if (gameObject.layer != requiredLayer)
            {
                Debug.Log($"[Harvestable] {name} : layer был'{LayerMask.LayerToName(gameObject.layer)}', " +
                          $"теперь принудительно установлен в {REQUIRE_LAYER_NAME}.", context: this);
                
                gameObject.layer = requiredLayer;
            } 
        }
        
        
    }
}