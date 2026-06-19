using UnityEngine; // Unity

namespace BuildSystem
{
    // Конвертирует позицию мыши в координаты тайла
    // MonoBehaviour — вешается на GameObject на сцене
    // Используется TilePainter и ToolModeController
    public class BuildInputHandler : MonoBehaviour
    {
        [SerializeField] private Camera _camera; // камера сцены (назначается в инспекторе)
        [SerializeField] private Grid _grid;     // сетка тайлмапа (назначается в инспекторе)

        // Возвращает координату клетки (Vector3Int) под курсором мыши
        public Vector3Int GetMouseCellPosition()
        {
            // ScreenToWorldPoint — переводим 2D позицию мыши в 3D мировые координаты
            Vector3 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);

            worldPos.z = 0; // убираем Z (у нас 2D, Z не нужен)

            // WorldToCell — переводим мировые координаты в координаты клетки тайлмапа
            return _grid.WorldToCell(worldPos);
        }
    }
}