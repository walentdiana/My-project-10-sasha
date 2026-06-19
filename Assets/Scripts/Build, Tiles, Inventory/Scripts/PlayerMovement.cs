using GameName.Input;    // нужен для InputComponent
using GameName.Pooling;  // нужен для SimplePool
using GameName.Prejectile; // нужен для Projectile
using UnityEngine;       // Unity
using UnityEngine.Tilemaps; // нужен для Tilemap

namespace GameName.Player
{
    // Движение игрока, прыжок, стрельба, клик по тайлам
    // MonoBehaviour — вешается на GameObject игрока
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;      // слои Unity которые считаются "землёй"
        private float _groundCheckDistance = 0.5f;            // радиус проверки земли под ногами
        private bool _bIsGrounded;                            // стоит ли на земле

        private PlayerComponent _playerComponent; // данные игрока (скорость, прыжок)
        private Rigidbody2D _rb;                  // физика

        [SerializeField] private Projectile _projectile;        // префаб снаряда (не используется)
        [SerializeField] private InputComponent _inputComponent; // чтение ввода
        [SerializeField] private SimplePool _pool;               // пул снарядов

        [SerializeField] private Tilemap _tilemap;          // тайлмап для клика (старая система)
        [SerializeField] private GameObject _spikePrefab;   // шип который ставится кликом

        // Awake — получаем компоненты с этого объекта
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerComponent = GetComponent<PlayerComponent>();
        }

        // Update — каждый кадр обрабатываем ввод
        private void Update()
        {
            // Прыжок: нажата кнопка И стоим на земле
            if (_inputComponent.GetJump() && _bIsGrounded)
            {
                // linearVelocity — текущая скорость. Меняем только Y (вверх)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _playerComponent.JumpForce);
            }

            if (_inputComponent.GetFire()) // стрельба (сейчас отключена в InputComponent)
            {
                Fire();
            }

            if (_inputComponent.GetClick()) // клавиша 1 — поставить шип
            {
                Click();
            }
        }

        // Ставит шип на тайл под курсором мыши (старая тестовая механика)
        private void Click()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            Camera cam = Camera.main; // переменная создана но не используется

            Debug.Log($"MouseClick is {worldPos}"); // логируем позицию клика

            Vector3Int cellPos = _tilemap.WorldToCell(worldPos); // мировые координаты → клетка
            Debug.Log(cellPos); // логируем клетку

            Vector3 cellCenter = _tilemap.GetCellCenterWorld(cellPos); // центр клетки в мире
            Instantiate(_spikePrefab, cellCenter, Quaternion.identity); // создаём шип (без поворота)
        }

        // FixedUpdate — для физики (вызывается с фиксированным интервалом)
        private void FixedUpdate()
        {
            // OverlapCircle — создаём круг под игроком, проверяем касается ли земли
            _bIsGrounded = Physics2D.OverlapCircle(transform.position, _groundCheckDistance, _groundLayer);

            Vector2 moveDir = _inputComponent.GetMove(); // получаем направление движения

            // Меняем только X скорость, Y оставляем (прыжок/падение)
            _rb.linearVelocity = new Vector2(moveDir.x * _playerComponent.Speed, _rb.linearVelocity.y);

            Flip(); // разворачиваем спрайт по направлению движения
        }

        // Разворачивает спрайт игрока в сторону движения
        private void Flip()
        {
            if (_rb.linearVelocity.x != 0) // если двигаемся
            {
                // Mathf.Sign: возвращает 1 если положительное, -1 если отрицательное
                // localScale.x = -1 разворачивает спрайт зеркально
                transform.localScale = new Vector3(Mathf.Sign(_rb.linearVelocity.x), 1, 1);
            }
        }

        // Берёт снаряд из пула и запускает его
        private void Fire()
        {
            var obj = _pool.Get();                   // берём снаряд из пула (не создаём новый)
            obj.transform.position = transform.position; // ставим на позицию игрока
            obj.transform.rotation = transform.rotation; // с поворотом игрока

            obj.OnTriggered += ProjectileHandler; // подписываемся: снаряд попал → вернуть в пул

            // Двигаем снаряд в ту сторону куда смотрит игрок
            obj.Move(new Vector2(transform.localScale.x, 0)); // localScale.x = 1 или -1
        }

        // Вызывается когда снаряд во что-то врезался
        private void ProjectileHandler(Projectile obj)
        {
            obj.OnTriggered -= ProjectileHandler; // отписываемся
            _pool.Return(obj);                    // возвращаем снаряд в пул
        }

        // OnDrawGizmosSelected — рисует отладочные фигуры в редакторе Unity
        // Видно только когда выбран этот объект
        private void OnDrawGizmosSelected()
        {
            if (transform) // если есть transform
            {
                // Красим зелёным если на земле, красным если в воздухе
                Gizmos.color = _bIsGrounded ? Color.green : Color.red;

                // Рисуем круг проверки земли
                Gizmos.DrawWireSphere(transform.position, _groundCheckDistance);
            }
        }
    }
}