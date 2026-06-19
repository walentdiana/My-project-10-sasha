using System.Collections.Generic; // нужен для Dictionary
using Inventory.Item;              // нужен для ItemObject
using UnityEngine;                 // Unity

namespace Inventory.ItemDatabase
{
    // База данных всех предметов игры — один .asset файл на весь проект
    // ISerializationCallbackReceiver — Unity вызывает методы до/после сохранения
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Database")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] Items; // массив всех предметов, заполняется в инспекторе

        // Dictionary — словарь для быстрого поиска по ID
        // Items[0] имеет ID=0, Items[1] имеет ID=1 и т.д.
        // Словарь: ключ=ID, значение=предмет → поиск за O(1) вместо перебора массива
        public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

        // Вызывается Unity ПОСЛЕ загрузки данных из файла
        // Пересобираем словарь (он не сохраняется, только массив Items сохраняется)
        public void OnAfterDeserialize()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Id = i;          // ID предмета = его индекс в массиве
                GetItem.Add(i, Items[i]); // кладём в словарь: ID → предмет
            }
        }

        // Вызывается Unity ПЕРЕД сохранением данных в файл
        // Очищаем словарь — он пересоберётся при следующей загрузке
        public void OnBeforeSerialize()
        {
            GetItem = new Dictionary<int, ItemObject>(); // сброс словаря
        }
    }
}