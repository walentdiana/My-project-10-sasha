using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
   [SerializeField] private Transform _player;
   [SerializeField] private float _smoothSpeed = 0.125f;
   [SerializeField] private Vector3 _offset;

   void LateUpdate()
   {
      if (_player)
      {
         Vector3 desiredPos = new Vector3(
            _player.position.x + _offset.x,
            _player.position.y + _offset.y,
            transform.position.z
            );
         
         Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, _smoothSpeed);
         transform.position = smoothedPos;
      }
   }
}
