using BuildSystem;
using Core.Building;
using Zenject;

public class Bootstraper : MonoInstaller
{
    public override void InstallBindings()
    {
        
        Container
            .BindInterfacesAndSelfTo<StaticInventoryView>()
            .FromComponentInHierarchy(includeInactive: true)
            .AsSingle();
        
        Container
            .Bind<IBuildModeController>()
            .To<BuildModeController>()
            .AsSingle();
        
        Container
            .Bind<IBuildRequestSource>()
            .To<StaticInventoryView>()
            .FromComponentInHierarchy()
            .AsSingle();
        
        Container
            .Bind<TilePainter>()
            .FromComponentInHierarchy()
            .AsSingle();
        
        Container
            .BindInterfacesAndSelfTo<BuildModeController>()
            .AsSingle();
        
        Container
            .Bind<PaletteUIManager>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}