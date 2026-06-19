using System;
using Core.Building;
using UnityEngine;
using Zenject;

namespace BuildSystem
{
    public interface IBuildModeController
    {
        bool bIsActive { get; }
        BuildPalette CurrentPalette { get; }

        void Deactivate();
    }
    
    public class BuildModeController : IBuildModeController, IInitializable, IDisposable, ITickable
    {
        private PaletteUIManager _paletteUI;
        private TilePainter      _painter;
        private IBuildRequestSource _buildSource;
        
        
        private const  KeyCode VariantsKey = KeyCode.R;

        public BuildPalette CurrentPalette { get; private set; }
        public bool bIsActive { get; private set; }


        [Inject]
        public void Construct(
            PaletteUIManager paletteUI,
            TilePainter painter,
            IBuildRequestSource buildSource)
        {
            _paletteUI = paletteUI;
            _painter = painter;
            _buildSource = buildSource;
        }

        public void Initialize()
        {
            _buildSource.OnBuild += HandleBuild;
        }

        public void Dispose()
        {
            _buildSource.OnBuild -= HandleBuild;
        }

        private void HandleBuild(BuildPalette palette, int defaultTileIndex)
        {
            CurrentPalette = palette;
            bIsActive = true;
            
            var tileData = palette.Tiles[defaultTileIndex];
            _painter.SetActiveTile(tileData, palette.Type);
            
            _paletteUI.SetVisible(false);
        }

        public void Tick()
        {
            if (!bIsActive)
                return;
            
            if (Input.GetKeyDown(VariantsKey) && CurrentPalette != null)
                _paletteUI.ToggleFor(CurrentPalette);
        }

        public void Deactivate()
        {
            bIsActive = false;
            CurrentPalette = null;
            _painter.ClearActiveTile();
            _paletteUI.SetVisible(false);
        }
    }
}