using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(fileName = "New Tool Item", menuName = "Inventory System/Items/Tool")]
    public class ToolItemObject : ItemObject, IToolUsable
    {
        [field: SerializeField] public ToolCapability Capabilities { get; private set; }
        [field: SerializeField] public int Volume { get; private set; } 
    }
}