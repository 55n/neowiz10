using System;

namespace Darkness
{
    public class ActiveEffect
    {
        public EffectType Type { get; private set; }
        public int RemainingDuration { get; private set; }
        public int StackCount { get; private set; }

        public ActiveEffect(
            EffectType type,
            int remainingDuration,
            int stackCount)
        {
            Type = type;
            RemainingDuration = remainingDuration;
            StackCount = Math.Max(1, Math.Min(type.MaxStackCount, stackCount));
        }

        public void AddStack()
        {
            StackCount = Math.Min(Type.MaxStackCount, StackCount + 1);
        }

        public bool Tick(EffectDurationUnit unit)
        {
            if (Type.DurationScope == EffectDurationScope.Infinite || Type.DurationUnit != unit)
            {
                return false;
            }

            RemainingDuration = Math.Max(0, RemainingDuration - 1);
            return RemainingDuration == 0;
        }
    }
}
