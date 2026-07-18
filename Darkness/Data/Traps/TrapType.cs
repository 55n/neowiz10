using System;
using System.Collections.Generic;

namespace Darkness
{
    public class TrapType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Damage { get; private set; }
        public int Accuracy { get; private set; }
        public bool IsSingleUse { get; private set; }
        public List<EffectApplication> TriggerEffects { get; private set; }

        public TrapType(
            string id,
            string name,
            string description,
            int damage,
            int accuracy,
            bool isSingleUse,
            List<EffectApplication> triggerEffects)
        {
            Id = id;
            Name = name;
            Description = description;
            Damage = damage;
            Accuracy = accuracy;
            IsSingleUse = isSingleUse;
            TriggerEffects = triggerEffects;
        }
    }
}
