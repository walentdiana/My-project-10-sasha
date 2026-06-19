using System;
using Core.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Zenject;

namespace BuildSystem
{
    // Единственная ответственность: рисовать и стирать тайлы + управлять превью.
    // Получает команды от PaletteUIManager (SetActiveTile, PreviewTile, RestorePreview).
    // Сам читает ввод мыши и рисует активный тайл.
    public class TilePainter : MonoBehaviour
    {
        [SerializeField] private BuildInputHandler  _inputHandler;
        [SerializeField] private BuildPreviewSystem _previewSystem;

        private BuildTileData _activeTile;   // зафиксирован кликом по кнопке
        private BuildTileData _previewTile;  // временно при наведении на кнопку
        private Tilemap _activeTilemap;
        private Tilemap _previewTilemap;
        private TilemapLayerRegistry _layerRegistry;

        public event Action OnTilePlaced;

        [Inject]
        public void Construction(TilemapLayerRegistry layerRegistry)
        {
            _layerRegistry = layerRegistry;
        }

        public void SetActiveTile(BuildTileData data, TilemapLayerType layerType)
        {
            _activeTile = data;
            _activeTilemap = _layerRegistry.GetLayer(layerType);
            _previewSystem.SetPreviewSprite(data.Icon);
            _previewSystem.EnablePreview();
        }

        public void PreviewTile(BuildTileData data, TilemapLayerType layerType)
        {
            _previewTile = data;
            _previewTilemap = _layerRegistry.GetLayer(layerType);
            _previewSystem.SetPreviewSprite(data.Icon);
            _previewSystem.EnablePreview();
        }
        
        public void RestorePreview()
        {
            _previewTile = null;
            _previewTilemap = null;

            if (_activeTile)
                _previewSystem.SetPreviewSprite(_activeTile.Icon);
            else
                _previewSystem.DisablePreview();
        }

        public void ClearActiveTile()
        {
            _activeTile = null;
            _previewTile = null;
            _activeTilemap = null;
            _previewTilemap = null;
            _previewSystem.DisablePreview();
        }

        private void Update()
        {
            if (!_activeTile)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                _previewSystem.DisablePreview();
                return;
            }

            _previewSystem.EnablePreview();

            Vector3Int cell = _inputHandler.GetMouseCellPosition();
            BuildTileData current = _activeTile ? _activeTile : _previewTile;
            Tilemap currentLayer = _activeTilemap ? _activeTilemap : _previewTilemap;

            _previewSystem.UpdatePreview(cell, CanPaint(cell, currentLayer));

            if (Input.GetMouseButtonDown(0) && CanPaint(cell, currentLayer))
                Paint(cell, current, currentLayer);

            if (Input.GetMouseButton(1))
                Erase(cell, currentLayer);
        }

        private bool CanPaint(Vector3Int cell, Tilemap layer) =>
            layer && !layer.GetTile(cell);

        private void Paint(Vector3Int cell, BuildTileData data, Tilemap layer)
        {
            layer.SetTile(cell, data.Tile);
            OnTilePlaced?.Invoke();
        }

        private void Erase(Vector3Int cell, Tilemap layer) =>
            layer.SetTile(cell, null);
    }
}
