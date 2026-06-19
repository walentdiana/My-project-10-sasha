namespace Inventory.Item
{
    public interface IToolUsable
    {
        ToolCapability Capabilities { get; }
        int Volume { get; }
    }
}