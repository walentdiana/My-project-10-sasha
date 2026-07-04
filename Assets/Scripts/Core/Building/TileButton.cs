using UnityEngine;           // Unity
using UnityEngine.EventSystems; // нужен для EventTrigger
using UnityEngine.UI;        // нужен для Image

namespace BuildSystem
{
    // Кнопка одного тайла в  палитре строительства
    // Аналог InventorySlotView — только визуал, никакой логики
    // Логику кликов вешает TileEventBinder снаружи
    // [RequireComponent] — Unity добавит EventTrigger если его нет
    [RequireComponent(typeof(EventTrigger))]
    public class TileButton : MonoBehaviour
    {
        [SerializeField] private Image _icon; // иконка тайла на кнопке

        // Data — данные тайла, читать можно снаружи, менять нельзя
        public BuildTileData Data { get; private set; }

        // Инициализирует кнопку данными тайла
        // Вызывается из PaletteUIManager.Rebuild() при создании кнопки
        public void Initialize(BuildTileData data)
        {
            Data = data;             // запоминаем данные
            _icon.sprite = data.Icon; // ставим иконку
        }
    }
}