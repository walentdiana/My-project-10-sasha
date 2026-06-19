using Inventory.Container;
using Inventory.UI;
using UnityEngine;

namespace GameName.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerMovement))]
   public class PlayerComponent : MonoBehaviour
   {
      [SerializeField] private float _speed = 1f;
      internal float Speed => _speed;

      [SerializeField] private float _jumpForce = 1f;
      internal float JumpForce => _jumpForce;
     
      public MouseItems MouseItems =  new MouseItems();

      public InventoryObject inventory;
      
      
      private Rigidbody2D _rb;

      private void Awake()
      {
          _rb = GetComponent<Rigidbody2D>();
      }
   }
}
