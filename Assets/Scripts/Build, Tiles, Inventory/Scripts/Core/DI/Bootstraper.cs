using BuildSystem;                   // нужен для TilePainter, PaletteUIManager и др.
using BuildSystem.TileTimeDependent; // нужен для ITileTimeDependent, TileTimeDependentStub
using Core;                          // нужен для Initialize
using Core.Building;                 // нужен для TilemapLayerRegistry
using Core.ToolMode;                 // нужен для ToolModeController
using Data.Metadata;                 // нужен для TileMetadataRegistry
using Inventory.Core;                // нужен для StaticInventoryView
using UnityEngine;                   // нужен для SerializeField
using Zenject;                       // нужен для MonoInstaller

// MonoInstaller — специальный класс Zenject
// Вешается на SceneContext объект в Unity
// InstallBindings() — регистрируем все зависимости один раз при старте
// После этого Zenject сам создаёт нужные объекты и передаёт зависимости
public class Bootstraper : MonoInstaller
{
    // ScriptableObject который нельзя найти через FindObjectOfType (он не на сцене)
    // Назначается вручную в инспекторе
    [SerializeField] private TileMetadataRegistry _tileMetadataRegistry;

    // Здесь регистрируем всё что Zenject должен знать
    public override void InstallBindings()
    {
        // ============ ИНВЕНТАРЬ ============

        // StaticInventoryView реализует и IInventorySelectionSource
        // BindInterfacesAndSelfTo = зарегистрировать и как себя, и как все свои интерфейсы
        // Теперь когда кто-то просит IInventorySelectionSource — получит StaticInventoryView
        Container
            .BindInterfacesAndSelfTo<StaticInventoryView>()
            .FromComponentInHierarchy(includeInactive: true) // ищем на сцене (даже выключенный)
            .AsSingle(); // один на всю игру

        // ============ СТРОИТЕЛЬСТВО ============

        // TilePainter — MonoBehaviour, ищем его на сцене
        Container
            .Bind<TilePainter>()
            .FromComponentInHierarchy()
            .AsSingle();

        // PaletteUIManager — MonoBehaviour
        Container
            .Bind<PaletteUIManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        // BuildInputHandler — MonoBehaviour
        Container
            .Bind<BuildInputHandler>()
            .FromComponentInHierarchy()
            .AsSingle();

        // BuildModeController — НЕ MonoBehaviour, Zenject создаёт сам
        // BindInterfacesAndSelfTo — регистрирует и IBuildModeController, и ITickable, и IInitializable...
        // Zenject увидит IInitializable и сам вызовет Initialize()
        // Zenject увидит ITickable и сам будет вызывать Tick() каждый кадр
        Container
            .BindInterfacesAndSelfTo<BuildModeController>()
            .AsSingle();

        // ============ ИНСТРУМЕНТЫ ============

        // TileMetadataRegistry — ScriptableObject, передаём конкретный ассет из инспектора
        // FromScriptableObject — вместо создания нового берём существующий ассет
        Container
            .Bind<TileMetadataRegistry>()
            .FromScriptableObject(_tileMetadataRegistry)
            .AsSingle();

        // ITileTimeDependent — интерфейс системы времени
        // Bind<интерфейс>().To<реализация> — полиморфизм через DI
        // Сейчас используем заглушку, потом заменим на реальную систему
        Container
            .Bind<ITileTimeDependent>()
            .To<TileTimeDependentStub>() // TODO: заменить когда будет система времени
            .AsSingle();

        // ToolModeController — НЕ MonoBehaviour, Zenject управляет жизненным циклом
        Container
            .BindInterfacesAndSelfTo<ToolModeController>()
            .AsSingle();

        // TilemapLayerRegistry — MonoBehaviour
        Container
            .Bind<TilemapLayerRegistry>()
            .FromComponentInHierarchy()
            .AsSingle();

        // TileMetadataRegistryInitializer — вызовет registry.Initialize() при старте
        // BindInterfacesAndSelfTo — регистрирует IInitializable → Zenject вызовет Initialize()
        Container
            .BindInterfacesAndSelfTo<TileMetadataRegistryInitializer>()
            .AsSingle();

        // Initialize — MonoBehaviour для смены курсора мыши
        Container
            .Bind<Initialize>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}