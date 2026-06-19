using BuildSystem;
using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(fileName = "New Buildable Item", menuName = "Inventory System/Items/Buildable")]
    public class BuildableItemObject : ItemObject, IBuildable
    {
        [field: SerializeField] public BuildPalette LinkedPalette { get; private set; }
        [field: SerializeField] public int DefaultTileIndex { get; private set; }
    }
}