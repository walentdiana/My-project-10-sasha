using BuildSystem;
using BuildSystem.TileTimeDependent;
using Core;
using Core.Building;
using Core.ToolMode;
using Data.Metadata;
using Inventory.Core;
using UnityEngine;
using Zenject;

public class Bootstraper : MonoInstaller
{
  //  [SerializeField] private InventoryObject _inventoryObject;
    [SerializeField] private TileMetadataRegistry _tileMetadataRegistry;
    public override void InstallBindings()
    {
        // Инвентарь
        Container
            .BindInterfacesAndSelfTo<StaticInventoryView>()
            .FromComponentInHierarchy(includeInactive: true)
            .AsSingle();
 
        // Строительство
        Container
            .Bind<TilePainter>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .Bind<PaletteUIManager>()
            .FromComponentInHierarchy()
            .AsSingle();
        
        Container
            .Bind<BuildInputHandler>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<BuildModeController>()
            .AsSingle();

        // Инструменты (заглушки)
        Container
            .Bind<TileMetadataRegistry>()
            .FromScriptableObject(_tileMetadataRegistry)
            .AsSingle();
        
        Container
            .Bind<ITileTimeDependent>()
            .To<TileTimeDependentStub>()
            .AsSingle();
        
        Container
            .BindInterfacesAndSelfTo<ToolModeController>()
            .AsSingle();
        
        Container
            .Bind<TilemapLayerRegistry>()
            .FromComponentInHierarchy()
            .AsSingle();
        
        Container
            .BindInterfacesAndSelfTo<TileMetadataRegistryInitializer>()
            .AsSingle();
        
        Container
            .Bind<Initialize>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}