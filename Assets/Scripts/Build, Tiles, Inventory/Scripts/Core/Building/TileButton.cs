using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuildSystem
{
    // Аналог InventorySlotView — только визуал.
    // Никакой логики клика здесь нет — это делает TileEventBinder.
    // Initialize вызывается из PaletteUIManager.
    [RequireComponent(typeof(EventTrigger))]
    public class TileButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public BuildTileData Data { get; private set; }

        public void Initialize(BuildTileData data)
        {
            Data = data;
            _icon.sprite = data.Icon;
        }
    }
}