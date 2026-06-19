using System;           // нужен для Action
using GameName.Pooling; // нужен для SimplePool (не используется напрямую)
using UnityEngine;      // Unity

namespace GameName.Prejectile // опечатка в namespace: Prejectile вместо Projectile
{
    // Снаряд — летит в заданном направлении, при столкновении сообщает об этом
    // MonoBehaviour — вешается на GameObject снаряда
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;          // скорость полёта
        [SerializeField] private GameObject _projectilePrefab; // (не используется)

        // Событие: снаряд столкнулся — SimplePool.ProjectileHandler вернёт его в пул
        // Action<Projectile> — делегат который принимает Projectile и ничего не возвращает
        public Action<Projectile> OnTriggered;

        private Vector2 _direction = Vector2.zero; // направление полёта (изначально стоим)

        private Rigidbody2D _rb; // физика

        // Awake — получаем физику
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update — каждый кадр двигаем снаряд
        private void Update()
        {
            if (_direction != Vector2.zero) // если есть направление
            {
                // normalized — делает вектор единичной длины (убирает влияние величины)
                // * _speed — умножаем на скорость
                _rb.linearVelocity = _direction.normalized * _speed;
            }
        }

        // Устанавливает направление полёта — вызывается из PlayerMovement.Fire()
        public void Move(Vector2 direction)
        {
            _direction = direction;
        }

        // OnTriggerEnter2D — Unity вызывает когда коллайдер снаряда входит в другой
        // Collider2D other — тот кого задели
        private void OnTriggerEnter2D(Collider2D other)
        {
            // CompareTag — проверяем тег (быстрее чем other.tag == "...")
            if (other.CompareTag("Envirovment")) // опечатка: Envirovment вместо Environment
            {
                Debug.Log("Envirovment entered"); // лог для отладки
                OnTriggered?.Invoke(this);        // сообщаем что снаряд столкнулся (this = этот снаряд)
            }
        }
    }
}