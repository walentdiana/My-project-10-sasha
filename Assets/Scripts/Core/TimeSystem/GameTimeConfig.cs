using UnityEngine;

namespace Core.TimeSystem
{
    [CreateAssetMenu(fileName = "GameTimeConfig", menuName = "TimeSystem/GameTimeConfig", order = 0)]
    public class GameTimeConfig : ScriptableObject
    {
        [Header("CalendarStructure")]
        [SerializeField] private int _hourPerDay = 24;
        [SerializeField] private int _daysPerWeek = 7;
        [SerializeField] private int _weeksPerMonth = 4;
        
        [Header("Simulation speed")]
        [Tooltip("Реальных секунд на одну игровую минуту")]
        [SerializeField] private float _realSecondsPerGameMinutes = 1.0f;
        
        [Header("Start Time")]
        [SerializeField] private int _startHour = 6;
        
        public int HoursPerDay => _hourPerDay;
        public int DaysPerWeek => _daysPerWeek;
        public int WeeksPerMonth => _weeksPerMonth;

        public int DayPerMonth => _daysPerWeek * _weeksPerMonth;

        public float RealSecondsPerGameMinutes => _realSecondsPerGameMinutes;
        public float StartHour => _startHour;

        public int MinutesPerHour => 60;

        public int MinutesPerDay => MinutesPerHour * _hourPerDay;
        public int MinutesPerWeek => MinutesPerDay * _daysPerWeek;
        public int MinutesPerMonth => MinutesPerDay * DayPerMonth;
        
    }
}