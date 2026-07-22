using UnityEngine;
using Zenject;
namespace Core.TimeSystem
{
    public interface ITimeService
    {
        GameTime Now { get; }
        float TimeScale { get; set; }
        bool bIsPaused { get; set; }

    }


    /// <summary>
    /// Единственный тикающий компонент системы времени.
    /// Продвигает GameTime и сравнивает границы.
    /// Посылает сигналы на пересечнные границы.
    /// </summary>
    public sealed class TimeService : ITimeService, ITickable
    {
        private readonly GameTimeConfig _config;
        private readonly SignalBus _signalBus;


        private long _totalMinutes;
        private float _minutesAccumulated;


        public GameTime Now => new GameTime(_totalMinutes, _config);
        public float TimeScale { get; set; } = 1f;
        public bool bIsPaused { get; set; }


        [Inject]
        public TimeService(GameTimeConfig config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            _totalMinutes = (long)_config.StartHour * _config.MinutesPerHour;
        }
        public void Tick()
        {
            if(bIsPaused)
                return;


            var previous = Now;


            _minutesAccumulated += Time.deltaTime * TimeScale / _config.RealSecondsPerGameMinutes;
            if(_minutesAccumulated < 1f)
                return;


            int minutesToAdd = (int)_minutesAccumulated;
            _minutesAccumulated -= minutesToAdd;
            _totalMinutes += minutesToAdd;


            var current = Now;
            DispatchBoundaryCrossing(previous, current);

        }

        private void DispatchBoundaryCrossing(GameTime previous, GameTime current)
        {
            if(current.TotalHours == previous.TotalHours)
                return;

            _signalBus.Fire(new HourChangedSignal(current));


            if(current.Day == previous.Day)
                return;
            _signalBus.Fire(new DayChangedSignal(current));


            if(current.Week == previous.Week)
                return;
            _signalBus.Fire(new WeekChangedSignal(current));


            if(current.Month == previous.Month)
                return;
            _signalBus.Fire(new MonthChangedSignal(current));
        }
    }
}
