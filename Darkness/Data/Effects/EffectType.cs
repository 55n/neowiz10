namespace Darkness
{
    public class EffectType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int? Duration { get; private set; }
        public bool IsStackable { get; private set; }
        public int MaxStackCount { get; private set; }

        public EffectType(
            string id,
            string name,
            string description,
            int? duration,
            bool isStackable,
            int maxStackCount)
        {
            Id = id;
            Name = name;
            Description = description;
            Duration = duration;
            IsStackable = isStackable;
            MaxStackCount = maxStackCount;
        }
    }
}
