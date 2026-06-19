using System;
using GameName.Input;
using UnityEngine;

namespace GameName.Player
{
    public class PlayerAnimationComponent : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            bool bIsMoving = Mathf.Abs(_rb.linearVelocity.x) > 0.1f;
            _animator.SetBool("bIsMove", bIsMoving);
        }
    }
}