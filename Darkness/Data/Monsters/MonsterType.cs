using System.Collections.Generic;

namespace Darkness
{
    public class MonsterType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int MaxHealth { get; private set; }
        public int MaxFocus { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Accuracy { get; private set; }
        public int Evasion { get; private set; }
        public List<string> FocusSkillIds { get; private set; }
        public bool CanBePoisoned { get; private set; }

        public MonsterType(
            string id,
            string name,
            string description,
            int maxHealth,
            int maxFocus,
            int attack,
            int defense,
            int accuracy,
            int evasion,
            List<string> focusSkillIds,
            bool canBePoisoned = false)
        {
            Id = id;
            Name = name;
            Description = description;
            MaxHealth = maxHealth;
            MaxFocus = maxFocus;
            Attack = attack;
            Defense = defense;
            Accuracy = accuracy;
            Evasion = evasion;
            FocusSkillIds = focusSkillIds;
            CanBePoisoned = canBePoisoned;
        }
    }
}
