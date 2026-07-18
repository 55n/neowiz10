using System;

namespace Darkness
{
    public class HeroType
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

        public HeroType(
            string id,
            string name,
            string description,
            int maxHealth,
            int maxFocus,
            int attack,
            int defense,
            int accuracy,
            int evasion)
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
        }
    }
}
