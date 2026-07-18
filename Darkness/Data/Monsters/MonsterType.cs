using System;
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
        public int Speed { get; private set; }
        public int Accuracy { get; private set; }
        public int Evasion { get; private set; }
        public int InitialSearchTurns { get; private set; }
        public int MaxSearchTurns { get; private set; }
        public List<MonsterSense> Senses { get; private set; }
        public MonsterReactionSet Reactions { get; private set; }
        public List<EffectApplication> AttackEffects { get; private set; }
        public MonsterFunction AttackFunction { get; private set; }
        public List<string> FocusSkillIds { get; private set; }

        public MonsterType(
            string id,
            string name,
            string description,
            int maxHealth,
            int maxFocus,
            int attack,
            int defense,
            int speed,
            int accuracy,
            int evasion,
            int initialSearchTurns,
            int maxSearchTurns,
            List<MonsterSense> senses,
            MonsterReactionSet reactions,
            List<EffectApplication> attackEffects,
            MonsterFunction attackFunction,
            List<string> focusSkillIds)
        {
            Id = id;
            Name = name;
            Description = description;
            MaxHealth = maxHealth;
            MaxFocus = maxFocus;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Accuracy = accuracy;
            Evasion = evasion;
            InitialSearchTurns = initialSearchTurns;
            MaxSearchTurns = maxSearchTurns;
            Senses = senses;
            Reactions = reactions;
            AttackEffects = attackEffects;
            AttackFunction = attackFunction;
            FocusSkillIds = focusSkillIds;
        }
    }
}
