using BuildSystem;

namespace Inventory.Item
{
    public interface IBuildable
    {
        BuildPalette LinkedPalette { get; }
        int DefaultTileIndex { get; }
    }
}