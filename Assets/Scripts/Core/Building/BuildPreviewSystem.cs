using System;               // стандартная библиотека
using UnityEngine;          // Unity
using UnityEngine.Tilemaps; // нужен для Tilemap, Tile, TileFlags

namespace BuildSystem
{
    // Управляет превью тайла под курсором
    // Зелёный = можно поставить, Красный = клетка занята
    public class BuildPreviewSystem : MonoBehaviour
    {
        [Header("References")] // заголовок в инспекторе для красоты
        [SerializeField] private Tilemap _previewTilemap; // отдельный тайлмап только для превью

        [Header("Colors")]
        [SerializeField] private Color _canPlaceColor    = new Color(0, 1, 0, .5f); // зелёный полупрозрачный
        [SerializeField] private Color _cannotPlaceColor = new Color(1, 0, 0, .5f); // красный полупрозрачный

        private Tile _previewTile;           // тайл с иконкой выбранного предмета
        private Vector3Int _lastCellPosition; // последняя позиция превью (чтобы не обновлять каждый кадр)

        // Awake — вызывается при старте
        private void Awake()
        {
            Initialize();
        }

        // Создаёт временный тайл для превью
        private void Initialize()
        {
            // CreateInstance — создаём Tile в памяти (не из файла)
            _previewTile = ScriptableObject.CreateInstance<Tile>();
            _previewTilemap.color = Color.white; // полностью белый (не тонированный)
        }

        // Показывает тайлмап превью
        public void EnablePreview()
        {
            _previewTilemap.gameObject.SetActive(true); // включаем объект
        }

        // Скрывает тайлмап превью и очищает
        public void DisablePreview()
        {
            ClearPreview();                              // убираем тайл
            _previewTilemap.gameObject.SetActive(false); // выключаем объект
        }

        // Обновляет позицию и цвет превью
        public void UpdatePreview(Vector3Int cellPosition, bool canPlace)
        {
            if (_lastCellPosition == cellPosition)
                return; // позиция не изменилась — не тратим время зря

            ClearPreview(); // убираем старое превью

            _previewTilemap.SetTile(cellPosition, _previewTile); // ставим тайл на новую позицию

            // TileFlags.None — разрешаем менять цвет тайла программно
            _previewTilemap.SetTileFlags(cellPosition, TileFlags.None);

            // Красим в зелёный или красный в зависимости от того можно ли поставить
            _previewTilemap.SetColor(cellPosition, canPlace ? _canPlaceColor : _cannotPlaceColor);

            _lastCellPosition = cellPosition; // запоминаем новую позицию
        }

        // Убирает тайл с предыдущей позиции превью
        private void ClearPreview()
        {
            _previewTilemap.SetTile(_lastCellPosition, null); // null = убрать тайл
        }

        // Меняет спрайт тайла превью (иконку выбранного предмета)
        public void SetPreviewSprite(Sprite sprite)
        {
            _previewTile.sprite = sprite; // меняем иконку
        }
    }
}