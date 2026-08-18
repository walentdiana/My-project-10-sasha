using Inventory.Item;
using UnityEngine;


namespace Data.Harvestable
{
    [CreateAssetMenu(fileName = "HarvestableData", menuName = "Harvestable/HarvestableData")]
    public class HarvestableData : ScriptableObject
    {
        [field:SerializeField] private int _maxHP = 100;
        [SerializeField] private ToolCapability _requiredCapability;
        [field: SerializeField] public ItemDrop[] ItemDrops { get; private set; } 
        [SerializeField] private GameObject _vfxPrefab;


        [SerializeField] private bool bIsAllowsHandHarvest = true;
        [SerializeField] private int _handDamage = 1;


        public int MaxHP => _maxHP;
        public ToolCapability RequiredCapability => _requiredCapability;
        public GameObject VfxPrefab => _vfxPrefab;
        public bool IsAllowsHandHarvest => bIsAllowsHandHarvest;
        public int HandDamage => _handDamage;
    }
}