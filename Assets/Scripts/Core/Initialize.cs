using System;       // стандартная библиотека
using UnityEngine;  // Unity

namespace Core
{
    // Управляет курсором мыши
    // MonoBehaviour — вешается на GameObject на сцене
    // Zenject передаёт ссылку на него в ToolModeController
    public class Initialize : MonoBehaviour
    {
        [SerializeField] private Texture2D ToolCursor;     // текстура курсора когда активен инструмент
        [SerializeField] private Texture2D OrdinaryCursor; // обычный курсор

        private bool _bIsToolCursor; // true = сейчас показываем курсор инструмента

        // Start — при старте ставим обычный курсор
        private void Start()
        {
            // SetCursor: texture=текстура, hotspot=точка клика, mode=режим
            // Vector2.zero = точка клика в левом верхнем углу курсора
            Cursor.SetCursor(OrdinaryCursor, Vector2.zero, CursorMode.Auto);
        }

        // Переключает курсор — вызывается из ToolModeController
        // при активации/деактивации режима инструментов
        public void ChangeCursor()
        {
            _bIsToolCursor = !_bIsToolCursor; // инвертируем флаг

            // Тернарный оператор: если инструментальный режим — ToolCursor, иначе — OrdinaryCursor
            var currentCursor = (_bIsToolCursor) ? ToolCursor : OrdinaryCursor;

            Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}