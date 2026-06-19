using System;                        // нужен для IDisposable
using BuildSystem;                   // нужен для BuildInputHandler, TilemapLayerType
using BuildSystem.TileTimeDependent; // нужен для ITileTimeDependent
using Core.Building;                 // нужен для TilemapLayerRegistry
using Data.Metadata;                 // нужен для TileMetadataRegistry, TileMetadata
using Inventory.Container;           // нужен для InventorySlot
using Inventory.Item;                // нужен для ToolItemObject
using UnityEngine;                   // нужен для Input, KeyCode
using UnityEngine.EventSystems;      // нужен для EventSystem
using UnityEngine.Tilemaps;          // нужен для Tilemap
using Zenject;                       // нужен для IInitializable, ITickable, [Inject]

namespace Core.ToolMode
{
    // Контроллер режима инструментов (лопата, топор, лейка)
    // НЕ MonoBehaviour — чистый C# класс управляемый Zenject
    public class ToolModeController : IInitializable, IDisposable, ITickable
    {
        private IInventorySelectionSource _selectionSource; // хотбар — откуда выбирается инструмент
        private TileMetadataRegistry _metadataRegistry;    // что можно делать с каждым тайлом
        private TilemapLayerRegistry _layerRegistry;       // какой Tilemap у каждого слоя
        private ITileTimeDependent _timeSystem;            // система времени (пока заглушка)
        private BuildInputHandler _inputHandler;           // мышь → координата клетки
        private Initialize _initialize;                    // смена курсора мыши

        private ToolItemObject _activeTool;  // активный инструмент
        private InventorySlot _activeSlot;   // слот из которого взят инструмент
        private bool _isActive;              // включён ли режим
        private int _currentDurability;      // сколько кликов осталось до износа

        // Zenject передаёт все зависимости
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

        // Подписываемся на событие выбора предмета при старте
        public void Initialize()
        {
            _selectionSource.OnItemSelected += HandleItemSelected;
        }

        // Отписываемся при уничтожении
        public void Dispose()
        {
            _selectionSource.OnItemSelected -= HandleItemSelected;
        }

        // Игрок кликнул на предмет в хотбаре
        private void HandleItemSelected(InventorySlot slot)
        {
            // is not ToolItemObject — не инструмент? Игнорируем.
            if (slot.item?.Source is not ToolItemObject tool)
                return;

            _activeTool = tool;                 // запоминаем инструмент
            _activeSlot = slot;                 // запоминаем слот
            _isActive = true;                   // включаем режим
            _currentDurability = tool.Volume;   // восстанавливаем прочность
            _initialize.ChangeCursor();         // меняем курсор на "инструментальный"
        }

        // Каждый кадр через Zenject
        public void Tick()
        {
            if (!_isActive) return; // режим не активен

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactivate(); // Escape — выйти
                return;
            }

            // Мышь над UI (кнопкой инвентаря и т.д.) — не реагируем на клик
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0)) // левая кнопка мыши
                TryUseTool();
        }

        // Пробует применить инструмент к тайлу под курсором
        private void TryUseTool()
        {
            Vector3Int cell = _inputHandler.GetMouseCellPosition(); // клетка под мышью

            // Ищем метаданные для тайла в этой клетке
            // out TileMetadata metadata — выходной параметр: найденные метаданные
            if (!_metadataRegistry.TryGetMetadataAtCell(cell, _layerRegistry, out TileMetadata metadata))
                return; // нет метаданных — этот тайл не интерактивный

            // Проверяем: наш инструмент умеет то что нужно этому тайлу?
            // & — побитовое И: есть ли общие флаги?
            // == 0 значит общих флагов нет — инструмент не подходит
            if ((metadata.RequiredCapability & _activeTool.Capabilities) == 0)
                return; // лопата не рубит деревья, топор не копает грядки

            ApplyTool(cell, metadata); // применяем инструмент
        }

        // Применяет эффект инструмента: заменяет тайл на результирующий
        private void ApplyTool(Vector3Int cell, TileMetadata metadata)
        {
            // Получаем тайлмап результирующего слоя
            if (!_layerRegistry.TryGetLayer(metadata.ResultLayer, out Tilemap resultLayer))
                return; // нет такого слоя

            if (resultLayer.GetTile(cell)) // на результирующем слое уже что-то есть?
                return; // не перезаписываем

            resultLayer.SetTile(cell, metadata.ResultTile); // ставим результирующий тайл

            // Тайл должен меняться со временем? Регистрируем его
            if (metadata.TimeToNextState > 0)
                _timeSystem.RegisterTimedTile(cell, metadata.ResultLayer, metadata);

            ConsumeDurability(); // тратим прочность инструмента
        }

        // Расходует прочность; если кончилась — тратит предмет из стака
        private void ConsumeDurability()
        {
            _currentDurability--; // уменьшаем прочность на 1

            if (_currentDurability > 0) return; // ещё есть прочность — ничего не делаем

            // Прочность = 0: тратим один предмет из стака
            // TryConsume: убирает 1 штуку, возвращает false если стак пуст
            if (!_activeSlot.TryConsume())
            {
                Deactivate(); // стак закончился — выходим из режима
                return;
            }

            // Стак не пуст — перезаряжаем прочность для следующего предмета
            _currentDurability = _activeTool.Volume;
        }

        // Выключает режим инструментов
        private void Deactivate()
        {
            _isActive = false;
            _activeTool = null;
            _activeSlot = null;
            _initialize.ChangeCursor(); // возвращаем обычный курсор
        }
    }
}