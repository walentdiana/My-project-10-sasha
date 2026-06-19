using System;
using Core.ToolMode;
using Inventory.Container;
using Inventory.Item;
using UnityEngine;
using Zenject;

namespace BuildSystem
{
    
    public interface IBuildModeController
    {
        bool IsActive { get; }
        BuildPalette CurrentPalette { get; }

        void Deactivate();
    }
    
    // Единственная ответственность: включить/выключить режим строительства.
    // Не знает про тайлы, палитры или рисование.
    public class BuildModeController : IBuildModeController, IInitializable, IDisposable, ITickable
    {
        private IInventorySelectionSource _selectionSource;
        private TilePainter _painter;
        private PaletteUIManager _variantsUI;
        
        private const KeyCode VariantsKey = KeyCode.R;

        public bool IsActive { get; private set; }
        public BuildPalette CurrentPalette { get; private set; }
        private InventorySlot _activeSlot;

        [Inject]
        public void Construction(
            IInventorySelectionSource selectionSource,
            TilePainter painter, 
            PaletteUIManager variantsUI)
        {
            _selectionSource = selectionSource;
            _painter = painter;
            _variantsUI = variantsUI;
        }

        public void Initialize()
        {
            _selectionSource.OnItemSelected += HandleItemSelected;
            _painter.OnTilePlaced += HandleTilePlaced;
        }

        public void Dispose()
        {
            _selectionSource.OnItemSelected -= HandleItemSelected;
            _painter.OnTilePlaced -= HandleTilePlaced;
        }

        private void HandleTilePlaced()
        {
            if (_activeSlot == null)
                return;

            if (!_activeSlot.TryConsume())
                Deactivate();
        }

        private void HandleItemSelected(InventorySlot slot)
        {
            if (slot.item?.Source is not IBuildable buildable)
                return;

            CurrentPalette = buildable.LinkedPalette;
            IsActive = true;
            _activeSlot = slot;

            _painter.SetActiveTile(
                buildable.LinkedPalette.Tiles[buildable.DefaultTileIndex],
                buildable.LinkedPalette.LayerType);

            _variantsUI.SetVisible(false);
        }

        public void Tick()
        {
            if (!IsActive)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactivate();
                return;
            }

            if (Input.GetKeyDown(VariantsKey) && CurrentPalette)
                _variantsUI.ToggleFor(CurrentPalette);
        }

        public void Deactivate()
        {
            IsActive = false;
            CurrentPalette = null;
            _painter.ClearActiveTile();
            _variantsUI.SetVisible(false);
        }
    
    }
}
