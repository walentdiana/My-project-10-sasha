using UnityEngine;          // подключаем Unity — без этого не работает ничего
using Random = UnityEngine.Random; // говорим: Random — это Unity-шный Random, не C#-шный

namespace Inventory.Item   // папка в коде, чтобы не путаться с другими классами
{
    // Перечисление — просто список именованных чисел
    // Food=0, Equipment=1, Default=2 — Unity покажет их как выпадающий список
    public enum ItemType
    {
        Food,       // еда
        Equipment,  // снаряжение
        Default     // ничего особенного
    }

    // Ещё одно перечисление — характеристики персонажа
    public enum Attributes
    {
        Agility,        // ловкость
        Stamina,        // выносливость
        Strength,       // сила
        Intelligence,   // интеллект
        Health,         // здоровье
        Default         // нет атрибута
    }

    // abstract = нельзя создать ItemObject напрямую
    // ScriptableObject = хранится как .asset файл в папке проекта (не на сцене)
    // Это ШАБЛОН предмета — как карточка в книге рецептов
    public abstract class ItemObject : ScriptableObject
    {
        public int Id;              // номер предмета — проставляется базой данных автоматически
        public Sprite uiDisplay;    // картинка предмета (иконка в инвентаре)
        public string Name;         // название предмета ("Лопата", "Яблоко")
        [TextArea(15, 20)]          // делает большое поле для текста в инспекторе Unity
        public string Description;  // описание предмета
        public int MaxStack;        // сколько штук влезает в одну ячейку инвентаря
        public ItemCategory Category; // к какой категории относится (инструмент? еда? оружие?)

        public ItemAttributes[] ItemAttributesConfig; // массив характеристик этого предмета

        // Фабричный метод — создаёт реальный предмет из этого шаблона
        // Шаблон = рецепт. Item = блюдо, приготовленное по рецепту
        public Item CreateItem()
        {
            Item newItem = new Item(this); // создаём Item, передаём себя как шаблон
            return newItem;               // возвращаем готовый предмет
        }
    }

    // [System.Serializable] — Unity умеет сохранять и загружать этот класс
    // Item — РЕАЛЬНЫЙ предмет который лежит у игрока в кармане
    // ItemObject — шаблон, Item — экземпляр
    [System.Serializable]
    public class Item
    {
        public int Id;              // ID берётся из шаблона
        public string Name;         // имя берётся из шаблона
        public int MaxStack;        // макс стак из шаблона
        public ItemAttributes[] ItemAttributesConfig; // характеристики с рандомными значениями

        // Source — ссылка на шаблон (нужна чтобы проверить: это IBuildable? IToolUsable?)
        // [field: SerializeField] — сериализует свойство без отдельного поля
        [field: SerializeField] public ItemObject Source { get; private set; }

        // Конструктор — вызывается когда пишем new Item(шаблон)
        // Копирует данные из шаблона в этот экземпляр
        public Item(ItemObject item)
        {
            Source = item;            // запоминаем шаблон
            Name = item.Name;         // копируем имя
            Id = item.Id;             // копируем ID
            MaxStack = item.MaxStack; // копируем макс стак

            // Создаём массив характеристик такого же размера как у шаблона
            ItemAttributesConfig = new ItemAttributes[item.ItemAttributesConfig.Length];

            for (int i = 0; i < ItemAttributesConfig.Length; i++)
            {
                // Для каждой характеристики создаём новый экземпляр с рандомным значением
                ItemAttributesConfig[i] = new ItemAttributes(
                    item.ItemAttributesConfig[i].min,  // минимум из шаблона
                    item.ItemAttributesConfig[i].max)  // максимум из шаблона
                {
                    Attributes = item.ItemAttributesConfig[i].Attributes // тип атрибута (Сила, Ловкость...)
                };
            }
        }
    }

    // Одна характеристика предмета (например Сила: случайное число от 5 до 10)
    [System.Serializable]
    public class ItemAttributes
    {
        public Attributes Attributes; // тип: Сила? Ловкость? Здоровье?
        public int value;             // текущее значение (рандом между min и max)
        public int min;               // минимально возможное значение
        public int max;               // максимально возможное значение

        // Конструктор — принимает диапазон и сразу генерирует случайное значение
        public ItemAttributes(int _min, int _max)
        {
            min = _min;           // запоминаем минимум
            max = _max;           // запоминаем максимум
            GeneratedValue();     // сразу считаем случайное число
        }

        // Генерирует случайное число в диапазоне [min, max)
        public void GeneratedValue()
        {
            value = Random.Range(min, max); // Unity-шный рандом
        }
    }
}