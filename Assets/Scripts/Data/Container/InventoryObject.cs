using System;              // нужен для Action (события)
using Inventory.Item;      // нужен для ItemObject
using Inventory.ItemDatabase; // нужен для ItemDatabaseObject
using UnityEngine;         // Unity

namespace Inventory.Container
{
    // Сам инвентарь — ScriptableObject, хранится как .asset
    // Содержит массив ячеек (InventorySlot[])
    // UI читает данные отсюда и рисует их на экране
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public ItemDatabaseObject database; // база данных — нужна для получения иконок по ID
        public Inventory Container;         // обёртка над массивом ячеек

        // Событие — срабатывает когда инвентарь изменился
        // UI подписывается и перерисовывается автоматически
       // public event Action OnChanged;

        // Добавляет предмет в инвентарь
        // Сначала ищет существующий стак, потом занимает пустую ячейку
        public void AddItem(Item.Item _item, int _amount)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                // Есть такой же предмет и стак не переполнен?
                if (Container.Items[i].ID == _item.Id)
                {
                    if (Container.Items[i].amount < Container.Items[i].item.MaxStack)
                    {
                        Container.Items[i].AddAmount(_amount); // добавляем к существующему
                        return; // нашли место — выходим
                    }
                }
            }
            SetEmptySlot(_item, _amount); // не нашли стак — занимаем пустую ячейку
        }

        // Перемещает предметы между ячейками (при drag-and-drop)
        public void MoveItem(InventorySlot item1, InventorySlot item2)
        {
            // Одинаковые предметы — пробуем слить стаки
            if (item1.item.Id == item2.item.Id)
            {
                int amount = item1.amount + item2.amount; // суммарное количество

                if (item2.amount < amount) // в item2 влезает больше
                {
                    item2.UpdateSlot(item2.ID, item2.item, amount); // кладём всё в item2
                    RemoveItem(item1.item);                          // очищаем item1
                    return;
                }
            }

            // Разные предметы — просто меняем местами через временную переменную
            InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount); // копия item2
            item2.UpdateSlot(item1.ID, item1.item, item1.amount); // item2 = item1
            item1.UpdateSlot(temp.ID, temp.item, temp.amount);    // item1 = старый item2
        }

        // Удаляет предмет из инвентаря по ссылке
        public void RemoveItem(Item.Item _item)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item == _item) // нашли нужную ячейку
                {
                    Container.Items[i].UpdateSlot(-1, null, 0); // -1 = пустой слот
                }
            }
        }

        // [ContextMenu] — кнопка в инспекторе Unity, правая кнопка на компоненте
        [ContextMenu("Clear")]
        public void Clean()
        {
            foreach (var sell in Container.Items)
            {
                RemoveItem(sell.item); // удаляем каждый предмет
            }
        }

        // Ищет первую пустую ячейку и кладёт туда предмет
        private void SetEmptySlot(Item.Item item, int amount)
        {
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].ID <= -1) // ID <= -1 значит ячейка пустая
                {
                    Container.Items[i].UpdateSlot(item.Id, item, amount);
                    return; // заняли одну ячейку — выходим (не заполняем несколько!)
                }
            }
        }
    }

    // Обёртка над массивом — нужна потому что Unity не умеет
    // сериализовать массивы напрямую в ScriptableObject
    [Serializable]
    public class Inventory
    {
        public InventorySlot[] Items = new InventorySlot[20]; // 20 ячеек по умолчанию
    }

    // Одна ячейка инвентаря
    // Хранит: какой предмет, сколько штук, допустимые типы
    [Serializable]
    public class InventorySlot
    {
        // [field: NonSerialized] — событие НЕ сохраняется в файл (это нормально)
        // UI подписывается сюда и перерисовывает слот когда он изменится
        [field: NonSerialized] public event Action OnChanged;

        public ItemType[] AllowedItems = new ItemType[0];         // (устаревшее) допустимые типы
        public ItemCategory AllowedCategories = ItemCategory.None; // допустимые категории (флаги)

        public int ID = -1;    // ID предмета в этой ячейке. -1 = ячейка пустая
        public Item.Item item; // сам предмет (null если пусто)
        public int amount;     // количество предметов в ячейке

        // Конструктор пустого слота
        public InventorySlot()
        {
            ID = -1;      // нет предмета
            item = null;  // нет предмета
            amount = 0;   // ноль штук
        }

        // Конструктор с данными — используется для создания временных копий при обмене
        public InventorySlot(int id, Item.Item item, int amount)
        {
            ID = id;
            this.item = item;   // this. — чтобы отличить поле от параметра (они совпадают по имени)
            this.amount = amount;
        }

        // Обновляет ячейку и уведомляет всех подписчиков (UI перерисуется)
        public void UpdateSlot(int id, Item.Item item, int amount)
        {
            ID = id;
            this.item = item;
            this.amount = amount;
            OnChanged?.Invoke(); // ?. — вызываем только если кто-то подписан
        }

        // Добавляет количество и уведомляет UI
        public void AddAmount(int value)
        {
            amount += value;     // amount = amount + value
            OnChanged?.Invoke(); // сообщаем UI что изменилось
        }

        // Проверяет можно ли положить этот предмет в эту ячейку
        public bool CanPlaceInSlot(ItemObject _item)
        {
            if (AllowedCategories == ItemCategory.None)
                return true; // нет ограничений — можно класть что угодно

            // Побитовое И: если у предмета есть хоть одна из разрешённых категорий — OK
            return (AllowedCategories & _item.Category) != 0;
        }
    }

    // Методы-расширения — добавляем метод к классу не трогая сам класс
    // this InventorySlot slot — значит вызывается как slot.TryConsume()
    public static class InventorySlotExtensions
    {
        // Тратит один предмет из стака
        // Возвращает true если ещё что-то осталось, false если стак закончился
        public static bool TryConsume(this InventorySlot slot)
        {
            if (slot.amount <= 1) // последний предмет в стаке
            {
                slot.UpdateSlot(-1, null, 0); // очищаем ячейку
                return false;                  // стак пуст
            }
            slot.AddAmount(-1); // уменьшаем на 1
            return true;        // ещё есть предметы
        }
    }
}