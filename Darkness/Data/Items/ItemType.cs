using System;
using System.Collections.Generic;

namespace Darkness
{
    public class ItemType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemCategory Category { get; private set; }
        public int Price { get; private set; }
        public int MaxStackCount { get; private set; }
        public bool IsUsable { get; private set; }
        public int Weight { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int MaxDurability { get; private set; }
        public string BoundSkillId { get; private set; }
        public List<EffectApplication> UseEffects { get; private set; }
        public List<EffectApplication> ThrowEffects { get; private set; }
        public ItemFunction UseFunction { get; private set; }
        public ItemFunction ThrowFunction { get; private set; }

        public ItemType(
            string id,
            string name,
            string description,
            ItemCategory category,
            int price,
            int maxStackCount,
            bool isUsable,
            int weight,
            int attack,
            int defense,
            int maxDurability,
            string boundSkillId,
            List<EffectApplication> useEffects,
            List<EffectApplication> throwEffects,
            ItemFunction useFunction,
            ItemFunction throwFunction)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            MaxStackCount = maxStackCount;
            IsUsable = isUsable;
            Weight = weight;
            Attack = attack;
            Defense = defense;
            MaxDurability = maxDurability;
            BoundSkillId = boundSkillId;
            UseEffects = useEffects;
            ThrowEffects = throwEffects;
            UseFunction = useFunction;
            ThrowFunction = throwFunction;
        }
    }
}
