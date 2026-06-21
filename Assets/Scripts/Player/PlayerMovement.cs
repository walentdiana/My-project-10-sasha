using System;
using Farm.Player;
using GameName.Input;
using GameName.Player;
using UnityEngine;

namespace Farm.Player
{
    public class PlayerMovementComponent : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private PlayerComponent _playerComponent;
        private InputComponent _playerInputComponent;

        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerComponent = GetComponent<PlayerComponent>();
            _playerInputComponent = GetComponent<InputComponent>();
        }

        
        private void FixedUpdate()
        {
            Vector2 moveDir = InputComponent.GetMove();
            
            _rigidbody.linearVelocity = new Vector2(
                moveDir.x * _playerComponent.Speed,
                moveDir.y * _playerComponent.Speed
            );
            
            Flip();
        }
        
        
        private void Flip()
        {
            if (_rigidbody.linearVelocity.x != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(_rigidbody.linearVelocity.x);
                transform.localScale = scale;
            }
        }
    }
}