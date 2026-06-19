using System;
using BuildSystem;
using BuildSystem.TileTimeDependent;
using Core.Building;
using Data.Metadata;
using Inventory.Container;
using Inventory.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Zenject;

namespace Core.ToolMode
{
    public class ToolModeController : IInitializable, IDisposable, ITickable
    {
        private IInventorySelectionSource _selectionSource;
        private TileMetadataRegistry _metadataRegistry;
        private TilemapLayerRegistry _layerRegistry;
        private ITileTimeDependent _timeSystem;
        private BuildInputHandler _inputHandler;
        private Initialize _initialize;

        private ToolItemObject _activeTool;
        private InventorySlot _activeSlot;
        private bool _isActive;
        private int _currentDurability;

        [Inject]
        public void Construction(
            IInventorySelectionSource selectionSource,
            TileMetadataRegistry metadataRegistry,
            TilemapLayerRegistry layerRegistry,
            ITileTimeDependent timeSystem,
            BuildInputHandler inputHandler,
            Initialize initialize)
        {
            _selectionSource = selectionSource;
            _metadataRegistry = metadataRegistry;
            _layerRegistry = layerRegistry;
            _timeSystem = timeSystem;
            _inputHandler = inputHandler;
            _initialize = initialize;
        }

        public void Initialize()
        {
            _selectionSource.OnItemSelected += HandleItemSelected;
        }

        public void Dispose()
        {
            _selectionSource.OnItemSelected -= HandleItemSelected;
        }

        private void HandleItemSelected(InventorySlot slot)
        {
            if (slot.item?.Source is not ToolItemObject tool)
                return;

            _activeTool = tool;
            _activeSlot = slot;
            _isActive = true;
            _currentDurability = tool.Volume;
            _initialize.ChangeCursor();
        }

        public void Tick()
        {
            if (!_isActive)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactivate();
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0))
                TryUseTool();
        }

        private void TryUseTool()
        {
            Vector3Int cell = _inputHandler.GetMouseCellPosition();

            if (!_metadataRegistry.TryGetMetadataAtCell(cell, _layerRegistry, out TileMetadata metadata))
                return;

            if ((metadata.RequiredCapability & _activeTool.Capabilities) == 0)
                return;

            ApplyTool(cell, metadata);
        }

        private void ApplyTool(Vector3Int cell, TileMetadata metadata)
        {
            if (!_layerRegistry.TryGetLayer(metadata.ResultLayer, out Tilemap resultLayer))
                return;
            
            if (resultLayer.GetTile(cell))
                return;

            resultLayer.SetTile(cell, metadata.ResultTile);

            if (metadata.TimeToNextState > 0)
                _timeSystem.RegisterTimedTile(cell, metadata.ResultLayer, metadata);

            ConsumeDurability();
        }

        private void ConsumeDurability()
        {
            _currentDurability--;

            if (_currentDurability > 0)
                return;

            // прочность кончилась — тратим один инструмент из стака
            if (!_activeSlot.TryConsume())
            {
                // стак закончился — деактивируем
                Deactivate();
                return;
            }

            // ещё есть инструменты в стаке — перезаряжаем прочность
            _currentDurability = _activeTool.Volume;
        }

        private void Deactivate()
        {
            _isActive = false;
            _activeTool = null;
            _activeSlot = null;
            _initialize.ChangeCursor();
        }
    }
}