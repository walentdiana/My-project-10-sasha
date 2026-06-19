using System;           // стандартная библиотека (не используется — можно удалить)
using GameName.Input;   // нужен для InputComponent (не используется — можно удалить)
using UnityEngine;      // Unity

namespace GameName.Player
{
    // Управляет анимациями игрока
    // MonoBehaviour — вешается на того же GameObject что и PlayerComponent
    public class PlayerAnimationComponent : MonoBehaviour
    {
        private Animator _animator;    // компонент анимации Unity
        private Rigidbody2D _rb;       // физика — смотрим скорость

        // Awake — получаем компоненты
        private void Awake()
        {
            _animator = GetComponent<Animator>();     // берём Animator с этого объекта
            _rb = GetComponent<Rigidbody2D>();        // берём Rigidbody2D с этого объекта
        }

        // Update — каждый кадр проверяем движется ли игрок
        private void Update()
        {
            // Mathf.Abs — модуль числа (убираем знак минус)
            // linearVelocity.x — горизонтальная скорость
            // > 0.1f — маленький порог чтобы анимация не дёргалась при скорости ~0
            bool bIsMoving = Mathf.Abs(_rb.linearVelocity.x) > 0.1f;

            // Передаём в Animator: параметр "bIsMove" = двигается ли игрок
            // В Animator Controller должен быть параметр с таким именем
            _animator.SetBool("bIsMove", bIsMoving);
        }
    }
}