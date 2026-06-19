using System;      // нужен для Serializable
using UnityEngine; // Unity

namespace Inventory.Core
{
    // Struct — маленькая структура данных (как class, но хранится на стеке, быстрее)
    // [Serializable] — видна в инспекторе Unity
    // Хранит настройки расположения ячеек в сетке
    [Serializable]
    public struct SlotPosition
    {
        public int X_START;               // X-позиция первой ячейки
        public int Y_START;               // Y-позиция первой ячейки
        public int X_SPACE_BETWEEN_ITEM;  // расстояние между ячейками по горизонтали
        public int NUMBER_OF_COLUMN;      // сколько ячеек в одной строке
        public int Y_SPACE_BETWEEN_ITEMS; // расстояние между ячейками по вертикали
    }

    // Полный инвентарь — слоты создаются динамически (не заданы заранее)
    // В отличие от StaticInventoryView здесь нет массива готовых GameObject-ов
    public class DynamicInventoryView : InventoryView
    {
        public SlotPosition SlotPosition; // настройки сетки (назначается в инспекторе)

        // Создаёт GameObject для каждой ячейки инвентаря
        public override void CreateSlots()
        {
            slots = inventory.Container.Items; // берём данные ячеек из ScriptableObject

            for (int i = 0; i < slots.Length; i++)
            {
                // Instantiate — создаёт копию префаба, transform — родитель в иерархии
                var obj = Instantiate(slotPrefab, transform);

                // Ставим ячейку на правильное место в сетке
                obj.GetComponent<RectTransform>().localPosition = CalculatePos(i);

                // Вешаем события drag-and-drop на созданный объект
                SlotEventBinder.BindSlotEvent(obj, slots[i], this);

                // Привязываем визуальный компонент ячейки к данным
                var view = obj.GetComponent<InventorySlotView>();
                view.Bind(slots[i], inventory.database);
            }
        }

        // Вычисляет позицию ячейки с номером i в сетке
        private Vector3 CalculatePos(int i)
        {
            return new Vector3(
                // X: начальная позиция + (ширина ячейки * номер столбца)
                // i % NUMBER_OF_COLUMN — остаток от деления = номер столбца
                // Пример: i=5, NUMBER_OF_COLUMN=4 → 5%4=1 → второй столбец
                SlotPosition.X_START + (SlotPosition.X_SPACE_BETWEEN_ITEM * (i % SlotPosition.NUMBER_OF_COLUMN)),

                // Y: начальная позиция - (высота ячейки * номер строки)
                // i / NUMBER_OF_COLUMN — целочисленное деление = номер строки
                // Минус — потому что Y в Unity идёт вниз
                SlotPosition.Y_START + (-SlotPosition.Y_SPACE_BETWEEN_ITEMS * (i / SlotPosition.NUMBER_OF_COLUMN)),

                0f // Z всегда 0 (2D UI)
            );
        }
    }
}