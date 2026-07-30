namespace Core.TimeSystem
{
    public readonly struct HourChangedSignal
    {
        public readonly GameTime CurrentTime;
        public HourChangedSignal(GameTime currentTime) => CurrentTime = currentTime;
    }

    public readonly struct DayChangedSignal
    {
        public readonly GameTime CurrentTime;
        public DayChangedSignal(GameTime currentTime) => CurrentTime = currentTime;
    }

    public readonly struct WeekChangedSignal
    {
        public readonly GameTime CurrentTime;
        public WeekChangedSignal(GameTime currentTime) => CurrentTime = currentTime;
    }

    public readonly struct MonthChangedSignal
    {
        public readonly GameTime CurrentTime;
        public MonthChangedSignal(GameTime currentTime) => CurrentTime = currentTime;
    }

    /// <summary>
    /// сигналы, которые говорят "запланированное действие наступило" 
    /// Конкретные системы (Crop Trees) фильтруются по тэгу
    /// </summary>
    public struct ScheduledActionDueSignal
    {
        public readonly int EntityId;
        public readonly int Tag;
        public readonly int Version;

        public ScheduledActionDueSignal(int entityId, int tag, int version)
        {
            EntityId = entityId;
            Tag = tag;
            Version = version;
        }

    }
}