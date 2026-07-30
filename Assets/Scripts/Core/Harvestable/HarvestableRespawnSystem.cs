using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using Core.TimeSystem;

namespace Core.Harvestable
{
    public enum HarvestableActionTag
    {
        Respawn = 200
    }


    public class HarvestableRespawnSystem : IInitializable, IDisposable
    {
        private readonly ITimeScheduler _scheduler;
        private readonly ITimeService _timeService;
        private readonly SignalBus _signalBus;
        private readonly Dictionary<int, Harvestable> _activeHarvestables =  new Dictionary<int, Harvestable>();
        
        [Inject]
        public HarvestableRespawnSystem(ITimeScheduler scheduler, ITimeService timeService, SignalBus signalBus)
        {
            _scheduler = scheduler;
            _timeService = timeService;
            _signalBus = signalBus;
        }

        public void Initialize() => _signalBus.Subscribe<ScheduledActionDueSignal>(OnActionDue);
        public void Dispose() => _signalBus.Unsubscribe<ScheduledActionDueSignal>(OnActionDue);

        private void OnActionDue()
        {
            throw new NotImplementedException();
        }
    }
}