namespace Inventory.Item
{
    // interface — контракт: "любой кто меня реализует, обязан иметь эти свойства"
    // ToolModeController не знает про ToolItemObject напрямую —
    // он работает через этот интерфейс. Легко добавить новый тип инструмента.
    public interface IToolUsable
    {
        ToolCapability Capabilities { get; } // что умеет инструмент (копать, рубить...)
        int Volume { get; }                  // прочность: сколько кликов на один предмет
    }
}