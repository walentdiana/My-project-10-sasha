namespace Core.TimeSystem
{
    public readonly struct HourChangedSignal
    {
        public readonly GameTime CurrentTime;
        public HourChangedSignal(GameTime currentTime) => CurrentTime = currentTime;
    }


    public struct SheduledActionDueSignal
    {

    }
}