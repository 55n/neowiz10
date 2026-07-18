namespace Darkness
{
    public class MonsterReaction
    {
        public MonsterReactionType Type { get; private set; }
        public int AlertChange { get; private set; }
        public string Description { get; private set; }

        public MonsterReaction(
            MonsterReactionType type,
            int alertChange,
            string description)
        {
            Type = type;
            AlertChange = alertChange;
            Description = description;
        }
    }
}
