using System;              // нужен для IDisposable
using Core.ToolMode;       // нужен для IInventorySelectionSource
using Inventory.Container; // нужен для InventorySlot
using Inventory.Item;      // нужен для IBuildable
using UnityEngine;         // нужен для KeyCode, Input
using Zenject;             // нужен для IInitializable, IDisposable, ITickable, [Inject]

namespace BuildSystem
{
    // Интерфейс: "у меня есть IsActive, CurrentPalette и Deactivate()"
    // Другие классы могут зависеть от этого интерфейса вместо конкретного класса
    public interface IBuildModeController
    {
        bool IsActive { get; }              // активен ли режим строительства
        BuildPalette CurrentPalette { get; } // текущая палитра
        void Deactivate();                   // выключить режим
    }

    // Контроллер режима строительства
    // IInitializable — Zenject вызовет Initialize() при старте (вместо Awake/Start)
    // IDisposable    — Zenject вызовет Dispose() при уничтожении (вместо OnDestroy)
    // ITickable      — Zenject вызовет Tick() каждый кадр (вместо Update)
    // НЕ MonoBehaviour — чистый C# класс, управляется Zenject
    public class BuildModeController : IBuildModeController, IInitializable, IDisposable, ITickable
    {
        private IInventorySelectionSource _selectionSource; // хотбар — отсюда приходит выбор предмета
        private TilePainter _painter;                       // рисует тайлы
        private PaletteUIManager _variantsUI;               // UI выбора тайла из палитры

        private const KeyCode VariantsKey = KeyCode.R; // клавиша открытия палитры

        public bool IsActive { get; private set; }              // включён ли режим
        public BuildPalette CurrentPalette { get; private set; } // текущая палитра
        private InventorySlot _activeSlot;                       // слот из которого взят предмет

        // Zenject передаёт зависимости через этот метод
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

        // Вызывается Zenject при старте — подписываемся на события
        public void Initialize()
        {
            _selectionSource.OnItemSelected += HandleItemSelected; // игрок выбрал предмет
            _painter.OnTilePlaced += HandleTilePlaced;             // тайл поставлен
        }

        // Вызывается Zenject при уничтожении — отписываемся (иначе утечка памяти)
        public void Dispose()
        {
            _selectionSource.OnItemSelected -= HandleItemSelected;
            _painter.OnTilePlaced -= HandleTilePlaced;
        }

        // Тайл поставлен — тратим предмет из инвентаря
        private void HandleTilePlaced()
        {
            if (_activeSlot == null) // нет активного слота — ничего не делаем
                return;

            // TryConsume: убирает 1 штуку, возвращает false если стак закончился
            if (!_activeSlot.TryConsume())
                Deactivate(); // предметы кончились — выходим из режима
        }

        // Игрок кликнул на предмет в хотбаре
        private void HandleItemSelected(InventorySlot slot)
        {
            // is not IBuildable — предмет не строительный? Игнорируем.
            // buildable — автоматически созданная переменная если проверка прошла
            if (slot.item?.Source is not IBuildable buildable)
                return;

            CurrentPalette = buildable.LinkedPalette; // запоминаем палитру предмета
            IsActive = true;                          // включаем режим
            _activeSlot = slot;                       // запоминаем слот

            // Говорим TilePainter что рисовать (тайл по умолчанию из палитры)
            _painter.SetActiveTile(
                buildable.LinkedPalette.Tiles[buildable.DefaultTileIndex], // тайл по умолчанию
                buildable.LinkedPalette.LayerType);                        // слой

            _variantsUI.SetVisible(false); // прячем UI (откроется по R)
        }

        // Вызывается каждый кадр через Zenject
        public void Tick()
        {
            if (!IsActive) return; // режим не активен — ничего не делаем

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactivate(); // Escape — выйти из режима
                return;
            }

            // R + есть активная палитра → переключить UI выбора тайла
            if (Input.GetKeyDown(VariantsKey) && CurrentPalette)
                _variantsUI.ToggleFor(CurrentPalette);
        }

        // Выключает режим строительства, всё сбрасывает
        public void Deactivate()
        {
            IsActive = false;             // режим выключен
            CurrentPalette = null;        // нет активной палитры
            _painter.ClearActiveTile();   // убираем превью тайла
            _variantsUI.SetVisible(false); // прячем UI
        }
    }
}