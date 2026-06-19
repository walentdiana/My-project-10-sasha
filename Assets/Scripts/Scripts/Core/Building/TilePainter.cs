using System;
using System.Collections.Generic;
using BuildSystem_V2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace BuildSystem
{
    public class TilePainter : MonoBehaviour
    {
        [SerializeField] private BuildInputHandler _inputHandler;
        [SerializeField] private BuildPreviewSystem _previewSystem;
        [SerializeField] private Tilemap _previewTilemap;

        private BuildTileData _activeTile;
        private BuildTileData _previewTile;
        private Tilemap _activeTilemap;
        
        private readonly List<TilemapLayer> _registerLayers = new();

        public void RegisterLayer(TilemapLayer layer)
        {
            _registerLayers.Add(layer);
        }

        private Tilemap GetLayer(TilemapLayerType type)
        {
            foreach (var layer in _registerLayers)
            {
                if(layer.Type == type)
                    return layer.Tilemap;
            }
            return null;
        }

        public void PreviewTile(BuildTileData tileData, TilemapLayerType layerType)
        {
            _previewTile =  tileData;
            _previewTilemap = GetLayer(layerType);
            _previewSystem.SetPreviewSprite(tileData.Icon);
            _previewSystem.EnablePreview();
        }

        public void RestorePreview()
        {
            _previewTile = null;
            if(_activeTile)
                _previewSystem.SetPreviewSprite(_activeTile.Icon);
            else
                _previewSystem.DisablePreview();
        }

        public void SetActiveTile(BuildTileData tileData, TilemapLayerType layerType)
        {
            _activeTile = tileData;
            _activeTilemap = GetLayer(layerType);
            _previewSystem.SetPreviewSprite(tileData.Icon);
            _previewSystem.EnablePreview();
        }

        public void ClearActiveTile()
        {
            _activeTile = null;
            _previewTile =  null;
            _previewTilemap = null;
            _previewSystem.DisablePreview();
        }

        private void Update()
        {
            if(!_activeTile)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                _previewSystem.DisablePreview();
                return;
            }
            _previewSystem.EnablePreview();

            Vector3Int cell = _inputHandler.GetMouseCellPosition();

            BuildTileData current = _previewTile ?? _activeTile;
            Tilemap currentLayer = _previewTilemap ?? _activeTilemap;
            
            _previewSystem.UpdatePreview(cell, CanPaint(cell, currentLayer));
            
            if(Input.GetMouseButtonDown(0))
                Paint(cell, current, currentLayer);
            
            if(Input.GetMouseButtonDown(1))
                Erase(cell, currentLayer);
        }

        private bool CanPaint(Vector3Int cell, Tilemap layer) =>
            !layer.GetTile(cell);

        private void Paint(Vector3Int cell, BuildTileData data, Tilemap layer) =>
            layer.SetTile(cell, data.Tile);
        
        private void Erase(Vector3Int cell, Tilemap layer) =>
            layer.SetTile(cell, null);
    }
}