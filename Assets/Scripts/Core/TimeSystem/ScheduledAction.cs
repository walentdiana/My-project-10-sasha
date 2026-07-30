using System;

namespace Core.TimeSystem
{
    /// <summary>
    /// Единица запланированного действия.
    /// Value - type не алоцируется отдельно, живет как элемент внутреннего массива кучи
    /// </summary>
    public readonly struct ScheduledAction:IComparable<ScheduledAction>

    {
        public readonly int TargetHour;
        public readonly int EntityId;
        public readonly int Tag;
        public readonly int Version;

        public ScheduledAction(int targetHour, int entityId, int tag, int version)
        {
            TargetHour = targetHour;
            EntityId = entityId;
            Tag = tag;
            Version = version;
        }
        
        public int CompareTo(ScheduledAction other) => TargetHour.CompareTo(other.TargetHour);
    }
}