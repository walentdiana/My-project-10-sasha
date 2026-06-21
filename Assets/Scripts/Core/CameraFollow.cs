using UnityEngine;          // Unity
using UnityEngine.Tilemaps; // импорт есть но не используется — можно удалить

// Скрипт следования камеры за игроком
// MonoBehaviour — вешается на GameObject камеры
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;     // Transform игрока (назначается в инспекторе)
    [SerializeField] private float _smoothSpeed = 0.125f; // скорость сглаживания (0=стоит, 1=мгновенно)
    [SerializeField] private Vector3 _offset;       // смещение камеры от игрока (например чуть выше)

    // LateUpdate — вызывается после Update всех объектов
    // Идеально для камеры: сначала игрок двигается, потом камера догоняет
    void LateUpdate()
    {
        if (_player) // если есть игрок (защита от null)
        {
            // Желаемая позиция = позиция игрока + смещение
            // transform.position.z — Z камеры не меняем (нам нужна глубина как есть)
            Vector3 desiredPos = new Vector3(
                _player.position.x + _offset.x,  // X с учётом смещения
                _player.position.y + _offset.y,  // Y с учётом смещения
                transform.position.z              // Z без изменений
            );

            // Lerp — плавная интерполяция между двумя значениями
            // Vector3.Lerp(от, до, скорость): чем больше _smoothSpeed, тем быстрее
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, _smoothSpeed);

            transform.position = smoothedPos; // двигаем камеру
        }
    }
}