using UnityEngine; // Unity

namespace Inventory.Item
{
    // Создаётся через меню: Inventory System -> Items -> Tool
    // Это шаблон инструмента (лопата, топор, лейка)
    // Наследует ItemObject (базовые данные) + реализует IToolUsable (умения инструмента)
    [CreateAssetMenu(fileName = "New Tool Item", menuName = "Inventory System/Items/Tool")]
    public class ToolItemObject : ItemObject, IToolUsable
    {
        // Capabilities — что умеет этот инструмент (Till? Chop? Water?)
        // [field: SerializeField] — видно в инспекторе, но изменить можно только здесь
        // { get; private set; } — читать можно снаружи, менять только внутри класса
        [field: SerializeField] public ToolCapability Capabilities { get; private set; }

        // Volume — прочность инструмента: сколько кликов до износа одного предмета
        // Например 10 — значит 10 раз лопатой, потом расходуется 1 штука из стака
        [field: SerializeField] public int Volume { get; private set; }
    }
}