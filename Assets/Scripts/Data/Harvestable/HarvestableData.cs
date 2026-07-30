using Inventory.Item;
using UnityEngine;


namespace Data.Harvestable
{
    [CreateAssetMenu(fileName = "HarvestableData", menuName = "Harvestable/HarvestableData", order = 0)]
    public class HarvestableData : ScriptableObject
    {
        [SerializeField] private int _maxHP = 100;
        [SerializeField] private ToolCapability _requiredCapability;
        [SerializeField] private ItemDrop[] _drops;
        [SerializeField] private GameObject _vfxPrefab;


        [SerializeField] private int _respawnDelayHours = 24;
        [SerializeField] private bool bIsAllowsHandHarvest = true;
        [SerializeField] private int _handDamage = 1;


        public int MaxHP => _maxHP;
        public ToolCapability RequiredCapability => _requiredCapability;
        public ItemDrop[] Drops => _drops;
        public GameObject VfxPrefab => _vfxPrefab;
        public int RespawnDelayHours => _respawnDelayHours;
        public bool IsAllowsHandHarvest => bIsAllowsHandHarvest;
        public int HandDamage => _handDamage;
    }
}