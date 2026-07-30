namespace Core.TimeSystem
{
    /// <summary>
    /// Единственный источник правды о текущем моменте игрового времени.
    /// Всё, кроме TotalMinutes - вычисляемые свойства, ничего не дублируется и не хранится
    /// Берет наш тик и из него считает минуты. 
    /// </summary>

    public readonly struct GameTime
    {
        public readonly long TotalMinutes;
        private readonly GameTimeConfig _config;


        public GameTime(long totalMinutes, GameTimeConfig config)
        {
            TotalMinutes = totalMinutes;
            _config = config;
        }


        public int Minutes => (int)(TotalMinutes % _config.MinutesPerHour);
        public int Hour => (int)(TotalMinutes / _config.MinutesPerHour % _config.HoursPerDay);
        public int TotalHours => (int)(TotalMinutes / _config.MinutesPerHour);
        public int Day => (int)(TotalMinutes / _config.MinutesPerDay);
        public int DayOfWeek => Day % _config.DaysPerWeek;
        public int Week => Day / _config.DaysPerWeek;
        public int Month => Day / _config.MinutesPerMonth; // на минуты не на дни?

        ///<summary> 0..1 прогресс текущего часа. Для плавной интерполяции света </summary>
        public float NormalizedHourProgress => Minutes / (float)_config.MinutesPerHour;


        ///<summary> 0..1 прогресс текущих суток. Для Day/Night контроллера </summary>
        public float NormalizedDayProgress => (Hour * _config.MinutesPerHour + Minutes) / (float)_config.MinutesPerDay;

        public GameTime AddMinutes(long minutes) => new GameTime(totalMinutes: TotalMinutes + minutes, _config);
        public GameTime AddHours(int hours) => AddMinutes(hours * _config.MinutesPerHour);
        public GameTime AddDays(int days) => AddMinutes(days * _config.MinutesPerDay);


        public override string ToString() => $"Month {Month}, Week {Week}, Day {DayOfWeek}, {Hour:D2}:{Minutes:D2}";
    }
}          