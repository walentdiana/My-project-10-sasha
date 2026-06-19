using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildSystem
{
    public class BuildPreviewSystem : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Tilemap _previewTilemap;

        [Header("Colors")] [SerializeField] private Color _canPlaceColor = new Color(0, 1, 0, .5f);
        [SerializeField] private Color _cannotPlaceColor = new Color(1, 0, 0, .5f);

        private Tile _previewTile;
        private Vector3Int _lastCellPosition;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _previewTile = ScriptableObject.CreateInstance<Tile>();
            _previewTilemap.color = Color.white;
        }

        public void EnablePreview()
        {
            _previewTilemap.gameObject.SetActive(true);
        }

        public void DisablePreview()
        {
            ClearPreview();
            _previewTilemap.gameObject.SetActive(false);
        }

        public void UpdatePreview(Vector3Int cellPosition, bool canPlace)
        {
            if (_lastCellPosition == cellPosition)
                return;

            ClearPreview();

            _previewTilemap.SetTile(cellPosition, _previewTile);
            _previewTilemap.SetTileFlags(cellPosition, TileFlags.None);
            _previewTilemap.SetColor(cellPosition, canPlace ? _canPlaceColor : _cannotPlaceColor);

            _lastCellPosition = cellPosition;
        }

        private void ClearPreview()
        {
            _previewTilemap.SetTile(_lastCellPosition, null);
        }

        public void SetPreviewSprite(Sprite sprite)
        {
            _previewTile.sprite = sprite;
        }

    }
}