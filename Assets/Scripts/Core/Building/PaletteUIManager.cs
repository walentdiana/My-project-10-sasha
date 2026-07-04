using UnityEngine;   // Unity
using UnityEngine.UI; // нужен для LayoutRebuilder

namespace BuildSystem
{
    // Управляет UI панелью выбора тайлов из палитры
    // Аналог InventoryView — создаёт кнопки и вешает события
    // Открывается по нажатию R когда активен режим строительства
    public class PaletteUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TilePainter _painter;    // кому говорить что выбрано
        [SerializeField] private TileButton _buttonPrefab; // префаб одной кнопки
        [SerializeField] private Transform _container;     // куда класть кнопки в иерархии
        [SerializeField] private GameObject _rootPanel;    // корневой объект панели (показать/скрыть)

        private BuildPalette _currentPalette; // текущая открытая палитра
        private bool _isVisible;              // видна ли панель

        // Start — прячем панель при старте
        private void Start()
        {
            SetVisible(false);
        }

        // Переключает панель для палитры — вызывается из BuildModeController по R
        public void ToggleFor(BuildPalette palette)
        {
            // Та же палитра и панель видна? → закрываем
            if (_isVisible && _currentPalette == palette)
            {
                SetVisible(false);
                return;
            }

            // Другая палитра или панель скрыта → перестраиваем и открываем
            _currentPalette = palette;
            Rebuild(palette); // создаём кнопки
            SetVisible(true);
        }

        // Пересоздаёт все кнопки для выбранной палитры
        private void Rebuild(BuildPalette palette)
        {
            ClearButtons(); // удаляем старые кнопки

            foreach (BuildTileData tileData in palette.Tiles) // для каждого тайла в палитре
            {
                TileButton button = Instantiate(_buttonPrefab, _container); // создаём кнопку
                button.Initialize(tileData); // инициализируем данными тайла

                // Вешаем события hover/click на кнопку
                TileEventBinder.BindTileButton(button.gameObject, tileData, palette.Id, palette.LayerType, this);
            }

            // ForceRebuildLayoutImmediate — пересчитывает UI layout сразу
            // Иначе кнопки могут перекрываться
            LayoutRebuilder.ForceRebuildLayoutImmediate(_container.GetComponent<RectTransform>());
        }

        // Мышь навела на кнопку тайла → показываем превью этого тайла
        public void OnEnter(BuildTileData tileData, int paletteId, FlagsTilemapLayerType layerType)
        {
            _painter.PreviewTile(tileData, layerType); // говорим TilePainter показать превью
        }

        // Мышь ушла с кнопки → возвращаем превью активного тайла
        public void OnExit()
        {
            _painter.RestorePreview(); // восстанавливаем предыдущее превью
        }

        // Кликнули на кнопку → выбрали этот тайл как активный
        public void OnSelect(BuildTileData tileData, int paletteId, FlagsTilemapLayerType layerType)
        {
            _painter.SetActiveTile(tileData, layerType); // устанавливаем активный тайл
            SetVisible(false);                            // закрываем панель после выбора
        }

        // Показывает или скрывает панель
        public void SetVisible(bool value)
        {
            _isVisible = value;
            _rootPanel.SetActive(value); // включаем/выключаем GameObject панели

            if (!value)
                _currentPalette = null; // сбрасываем текущую палитру при скрытии
        }

        // Удаляет все дочерние объекты (кнопки) из контейнера
        private void ClearButtons()
        {
            foreach (Transform child in _container) // перебираем дочерние объекты
                Destroy(child.gameObject);           // удаляем каждый
        }
    }
}