using UnityEngine;

namespace BuildSystem
{
    public class BuildInputHandler : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Grid _grid;
        
        
        public Vector3Int GetMouseCellPosition()
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            return _grid.WorldToCell(worldPos);
        }
    }
}