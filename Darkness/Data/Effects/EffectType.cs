namespace Darkness
{
    public class EffectType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public EffectCategory Category { get; private set; }
        public EffectDurationScope DurationScope { get; private set; }
        public int Duration { get; private set; }
        public EffectDurationUnit DurationUnit { get; private set; }
        public bool IsStackable { get; private set; }
        public int MaxStackCount { get; private set; }
        public EffectTrigger Trigger { get; private set; }
        public bool RemoveAfterTrigger { get; private set; }
        public EffectFunction Function { get; private set; }

        public EffectType(
            string id,
            string name,
            string description,
            EffectCategory category,
            EffectDurationScope durationScope,
            int duration,
            EffectDurationUnit durationUnit,
            bool isStackable,
            int maxStackCount,
            EffectTrigger trigger,
            bool removeAfterTrigger,
            EffectFunction function)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            DurationScope = durationScope;
            Duration = duration;
            DurationUnit = durationUnit;
            IsStackable = isStackable;
            MaxStackCount = maxStackCount;
            Trigger = trigger;
            RemoveAfterTrigger = removeAfterTrigger;
            Function = function;
        }
    }
}
