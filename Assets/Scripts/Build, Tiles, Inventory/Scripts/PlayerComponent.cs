using Inventory.Container; // нужен для InventoryObject
using UnityEngine;          // Unity

namespace GameName.Player
{
    // Данные игрока — скорость, сила прыжка, инвентарь
    // [RequireComponent] — Unity добавит Rigidbody2D и PlayerMovement если их нет
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
    public class PlayerComponent : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;    // скорость движения
        internal float Speed => _speed;                 // => сокращение для { get { return _speed; } }
        // internal — видно только в сборке

        [SerializeField] private float _jumpForce = 1f; // сила прыжка
        internal float JumpForce => _jumpForce;          // то же самое для прыжка

        // Ссылка на ScriptableObject инвентаря игрока
        // PlayerMovement и другие скрипты могут читать его
        public InventoryObject inventory;

        private Rigidbody2D _rb; // физический компонент (движение, прыжок)

        // Awake — вызывается при создании объекта
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>(); // получаем физику с этого объекта
        }
    }
}