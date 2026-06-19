using BuildSystem;
using Core.Inventory;
using UnityEngine;

namespace Inventory.Item
{
    [CreateAssetMenu(fileName = "New Buildable Item", menuName = "Inventory System/Items/Buildable", order = 0)]
    public class BuildItemObject : ItemObject, IBuildable
    {
        [field: SerializeField] public BuildPalette LinkedPalette { get; set; }
        [field: SerializeField] public int DefaultTileIndex { get; set; }
    }
}