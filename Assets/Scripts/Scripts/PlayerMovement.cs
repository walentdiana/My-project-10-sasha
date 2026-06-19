using GameName.Input;
using GameName.Pooling;
using GameName.Prejectile;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameName.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;
        private float _groundCheckDistance = 0.5f;
        private bool _bIsGrounded;

        private PlayerComponent _playerComponent;
        private Rigidbody2D _rb;
        [SerializeField] private Projectile  _projectile;
        [SerializeField] private InputComponent _inputComponent;
        [SerializeField] private SimplePool _pool;


        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private GameObject _spikePrefab;

        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerComponent = GetComponent<PlayerComponent>();
        }

        private void Update()
        {
            if (_inputComponent.GetJump() && _bIsGrounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _playerComponent.JumpForce);
            }

            if (_inputComponent.GetFire())
            {
                Fire();
            }

            if (_inputComponent.GetClick())
            {
                Click();
            }
        }

        private void Click()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Camera cam = Camera.main;
            Debug.Log($"MouseClick is {worldPos}");

            Vector3Int cellPos = _tilemap.WorldToCell(worldPos);
            Debug.Log(cellPos);
            
            Vector3 cellCenter = _tilemap.GetCellCenterWorld(cellPos);
            Instantiate(_spikePrefab, cellCenter, Quaternion.identity);
        }

        private void FixedUpdate()
        {
            _bIsGrounded = Physics2D.OverlapCircle(transform.position, _groundCheckDistance, _groundLayer);
          
            Vector2 moveDir = _inputComponent.GetMove();
            _rb.linearVelocity = new Vector2(moveDir.x * _playerComponent.Speed, _rb.linearVelocity.y);

            Flip();
        }

        private void Flip()
        {
            if (_rb.linearVelocity.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(_rb.linearVelocity.x), 1, 1);
            }
        }

        private void Fire()
        {
            var obj = _pool.Get();
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;

            obj.OnTriggered += ProjectileHandler;
            
            obj.Move(new Vector2(transform.localScale.x, 0));
        }

        private void ProjectileHandler(Projectile obj)
        {
            obj.OnTriggered -= ProjectileHandler;
            _pool.Return(obj);
        }

        private void OnDrawGizmosSelected()
        {
            if (transform)
            {
                Gizmos.color = _bIsGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(transform.position, _groundCheckDistance);
            }
        }
    }
}