using UnityEngine;
using UnityEngine.UI;

namespace BuildSystem
{
    public class PaletteUIManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private PaletteDatabase _database;
        
        [Header("References")]
        [SerializeField] private TilePainter _painter;
        [SerializeField] private TileButton _buttonPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _rootPanel;

        private void Start()
        {
            Rebuild();
            SetVisible(false);
        }

        public void Rebuild()
        {
            ClearButtons();
            foreach (var palette in _database.Palettes)
            {
                if(!palette.bIsUnloked)
                    continue;

                foreach (var tileData in palette.Tiles)
                {
                    TileButton button = Instantiate(_buttonPrefab, _container);
                    button.Initialize(tileData);
                    TileEventBinder.BindTileEvent(button.gameObject, tileData, palette.Id,  palette.Type, this);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_container.GetComponent<RectTransform>());
        }

        public void OnEnter(BuildTileData tileData, int paletteId, TilemapLayerType layerType)
        {
            if(_database.TryGetPalette(paletteId, out BuildPalette palette))
                _painter.PreviewTile(tileData, layerType);
        }

        public void OnExit()
        {
            _painter.RestorePreview();
        }

        public void OnSelect(BuildTileData tileData, int paletteId, TilemapLayerType layerType)
        {
            if(_database.TryGetPalette(paletteId, out BuildPalette palette))
                _painter.SetActiveTile(tileData, layerType);
        }

        public void SetVisible(bool value)
        {
            _rootPanel.SetActive(value);
            
            if(!value)
                _painter.ClearActiveTile();
        }

        private void ClearButtons()
        {
            foreach (Transform child in _container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}