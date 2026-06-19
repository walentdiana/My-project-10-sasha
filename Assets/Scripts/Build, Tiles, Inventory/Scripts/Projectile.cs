using System;
using GameName.Pooling;
using UnityEngine;

namespace GameName.Prejectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private GameObject _projectilePrefab;

        public Action<Projectile> OnTriggered;
        
        private Vector2 _direction = Vector2.zero;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_direction != Vector2.zero)
            {
                _rb.linearVelocity = _direction.normalized * _speed;
            }
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Envirovment"))
            {
                Debug.Log("Envirovment entered");
                OnTriggered?.Invoke(this);
            }
        }
    }
}