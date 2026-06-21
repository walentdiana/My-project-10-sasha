using System;       // стандартная библиотека C#
using UnityEngine;  // Unity

namespace Inventory.Item
{
    // [CreateAssetMenu] — добавляет пункт в меню Unity:
    // правая кнопка в Project -> Inventory System -> Items -> Item
    // Так создаются .asset файлы с данными предметов
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/Item")]
    public class FoodObject : ItemObject // наследует ItemObject — это еда
    {
        // Awake вызывается Unity при создании объекта
        // Сейчас тело пустое — пока нет логики для еды (съесть, восстановить HP и т.д.)
        public void Awake()
        {
            // TODO: добавить логику еды
        }
    }
}