using System;
using UnityEngine;

namespace Core
{
    public class Initialize : MonoBehaviour
    {
        [SerializeField] private Texture2D ToolCursor;
        [SerializeField] private Texture2D OrdinaryCursor;

        private bool _bIsToolCursor;
        
        private void Start()
        {
            Cursor.SetCursor(OrdinaryCursor, Vector2.zero, CursorMode.Auto);
        }

        public void ChangeCursor()
        {
            _bIsToolCursor = !_bIsToolCursor;
            var currentCursor = (_bIsToolCursor) ? ToolCursor : OrdinaryCursor;
            Cursor.SetCursor(currentCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}