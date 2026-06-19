using BuildSystem;

namespace Core.Inventory
{
    public interface IBuildable
    {
        BuildPalette LinkedPalette { get; }
        int DefaultTileIndex { get; }
    }
}