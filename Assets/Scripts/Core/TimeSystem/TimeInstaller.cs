using UnityEngine;
using Zenject;

namespace Core.TimeSystem
{
    public class TimeInstaller : MonoInstaller
    {
        [SerializeField] private GameTimeConfig _config;


        public override void InstallBindings()
        {
            DeclareSignals();
            
            Container
                .BindInstance(_config)
                .AsSingle();


            Container
                .BindInterfacesTo<TimeService>()
                .AsSingle();


            Container
                .BindInterfacesTo<TimeScheduler>()
                .AsSingle();
        }
        
        private void DeclareSignals()
        {
            SignalBusInstaller.Install(Container);


            Container.DeclareSignal<HourChangedSignal>();
            Container.DeclareSignal<DayChangedSignal>();
            Container.DeclareSignal<WeekChangedSignal>();
            Container.DeclareSignal<MonthChangedSignal>();
            Container.DeclareSignal<ScheduledActionDueSignal>();
        }
    }
}