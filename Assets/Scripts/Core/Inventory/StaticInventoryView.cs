using System;              // нужен для Action (событие)
using Core.ToolMode;       // нужен для IInventorySelectionSource
using Inventory.Container; // нужен для InventorySlot
using UnityEngine;         // Unity

namespace Inventory.Core
{
    // Панель быстрого доступа внизу экрана.
    // Слоты заранее созданы на сцене. Когда игрок кликает на предмет — кричит событием "выбран этот предмет".
    public class StaticInventoryView : InventoryView, IInventorySelectionSource
    {
        // Массив готовых GameObject-ов слотов — назначается в инспекторе
        // В отличие от DynamicInventoryView слоты уже созданы заранее на сцене
        public GameObject[] staticSlots;

        // Событие: "игрок кликнул на предмет в хотбаре"
        // Контроллеры строительства и инструментов подписываются сюда через Zenject
        public event Action<InventorySlot> OnItemSelected;

        // Реализация абстрактного метода из InventoryView
        // Не создаём новые объекты — берём готовые из массива staticSlots
        public override void CreateSlots()
        {
            var items = inventory.Container.Items; // берём ячейки данных из ScriptableObject

            for (int i = 0; i < staticSlots.Length; i++)
            {
                // Вешаем события drag-and-drop на каждый слот
                SlotEventBinder.BindSlotEvent(staticSlots[i], items[i], this);
                // Вешаем событие клика на каждый слот
                SlotEventBinder.BindClickEvent(staticSlots[i], items[i], this);
                // Привязываем визуальный компонент к данным (иконка + количество)
                staticSlots[i].GetComponent<InventorySlotView>().Bind(items[i], inventory.database);
            }
        }

        // Переопределяем реакцию на клик по ячейке
        public override void OnSlotClick(InventorySlot slot)
        {
            if (slot.item == null) // ячейка пустая — ничего не делаем
                return;

            // Уведомляем всех подписчиков что выбран этот предмет
            // BuildModeController: "это IBuildable? включаем строительство"
            // ToolModeController:  "это ToolItemObject? включаем инструмент"
            OnItemSelected?.Invoke(slot);
        }
    }
}