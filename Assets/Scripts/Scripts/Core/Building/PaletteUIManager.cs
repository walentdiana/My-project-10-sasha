using UnityEngine;
using UnityEngine.UI;

namespace BuildSystem
{
    public class PaletteUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TilePainter _painter;
        [SerializeField] private TileButton  _buttonPrefab;
        [SerializeField] private Transform   _container;
        [SerializeField] private GameObject  _rootPanel;

        private BuildPalette _currentPalette;
        private bool _isVisible;
        
        private void Start()
        {
            SetVisible(false);
        }
        
        public void ToggleFor(BuildPalette palette)
        {
            if (_isVisible && _currentPalette == palette)
            {
                SetVisible(false);
                return;
            }

            _currentPalette = palette;
            Rebuild(palette);
            SetVisible(true);
        }
        
        public void Rebuild(BuildPalette palette)
        {
            ClearButtons();
            foreach (BuildTileData tileData in palette.Tiles)
            {
                TileButton button = Instantiate(_buttonPrefab, _container);
                button.Initialize(tileData);
                TileEventBinder.BindTileEvent(button.gameObject, tileData, palette.Id, palette.Type, this);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_container.GetComponent<RectTransform>());
        }

        public void OnEnter(BuildTileData tileData, int paletteId, TilemapLayerType layerType)
        {
             _painter.PreviewTile(tileData, layerType);
        }

        public void OnExit()
        {
            _painter.RestorePreview();
        }

        public void OnSelect(BuildTileData tileData, int paletteId, TilemapLayerType layerType)
        {
            _painter.SetActiveTile(tileData, layerType);
            SetVisible(false);
        }

        public void SetVisible(bool value)
        {
            _isVisible = value;
            _rootPanel.SetActive(value);

            if (!value)
                _currentPalette = null;
        }

        private void ClearButtons()
        {
            foreach (Transform child in _container)
                Destroy(child.gameObject);
        }
    }
}