using Inventory.Item; // нужен для ItemObject
using UnityEditor;    // нужен для EditorUtility (только в редакторе!)
using UnityEngine;    // Unity

namespace Inventory.Core
{
    // Предмет лежащий на земле — ждёт когда игрок подберёт его
    // ISerializationCallbackReceiver — Unity вызывает методы до/после сохранения
    // Используется чтобы сразу видеть спрайт предмета в редакторе
    public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
    {
        public ItemObject item; // какой предмет лежит на земле (назначается в инспекторе)

        // Вызывается Unity ПЕРЕД сохранением сцены
        // Автоматически обновляет спрайт SpriteRenderer чтобы совпадал с item.uiDisplay
        public void OnBeforeSerialize()
        {
            // GetComponentInChildren — ищет компонент на дочерних объектах
            GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;

            // SetDirty — говорит Unity "этот объект изменился, сохрани его"
            // Без этого изменение спрайта не сохранится в файл сцены
            EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        }

        // Вызывается Unity ПОСЛЕ загрузки данных
        // Сейчас пустой — ничего не нужно делать при загрузке
        public void OnAfterDeserialize()
        {
            // пусто
        }
    }
}