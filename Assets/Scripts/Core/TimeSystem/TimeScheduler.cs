using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Core.TimeSystem
{
    public interface ITimeScheduler
    {
        /// <summary>
        /// Планирует действие на target hour
        /// </summary>
        /// <param name="entityId">Id сущности</param>
        /// <param name="tag">Tag сущности</param>
        /// <param name="targetHour">Время срабатывания</param>
        /// <param name="jitterRangeHours">Случайный джиттер в часах, если задан</param>
        void Schedule(int entityId, int tag, int targetHour, int jitterRangeHours = 0);


        /// <summary>
        /// Отменяет все запланированные действия для сущности
        /// </summary>
        void Cancel(int entityId);
    }

    /// <summary>
    /// Единый переиспользуемый планировщик для растений, деревьев, событий и т.д
    /// Ничего не знает о конкретных подсистемах - только рассылает ActionDueSignal.
    /// </summary>
    public sealed class TimeScheduler : ITimeScheduler, IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly MinHeap<ScheduledAction> _heap = new MinHeap<ScheduledAction>();
        private readonly Dictionary<int, int> _currentVersion = new Dictionary<int, int>();


        [Inject]
        public TimeScheduler(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }


        public void Initialize() => _signalBus.Subscribe<HourChangedSignal>(OnHourChanged);
        public void Dispose() => _signalBus.Unsubscribe<HourChangedSignal>(OnHourChanged);
        
        public void Schedule(int entityId, int tag, int targetHour, int jitterRangeHours = 0)
        {
            int jitter = jitterRangeHours > 0
                ? Random.Range(-jitterRangeHours, jitterRangeHours + 1)
                : 0;


            int version = _currentVersion.TryGetValue(entityId, out var v) ?  v + 1 : 1;
            _currentVersion[entityId] = version;


            _heap.Push(new ScheduledAction( targetHour: targetHour + jitter, entityId, tag, version));
        }


        public void Cancel(int entityId)
        {
            int version = _currentVersion.TryGetValue(entityId, out var v) ?  v + 1 : 1;
            _currentVersion[entityId] = version;
        }
        
        private void OnHourChanged(HourChangedSignal signal)
        {
            int currentHour = signal.CurrentTime.TotalHours;


            while (_heap.Count > 0 && _heap.Peek().TargetHour <= currentHour)
            {
                var action = _heap.Pop();


                bool bIsStale = !_currentVersion.TryGetValue(action.EntityId, out var currentVersion)
                                || currentVersion != action.Version;


                if(bIsStale) continue;
                _signalBus.Fire(new ScheduledActionDueSignal(action.EntityId, action.Tag, action.Version));
            }
        }
    }
}