using System;
using System.Collections.Generic;

namespace Darkness
{
    public class BreathHunter : Monster
    {
        public const string TypeId = "breath_hunter";

        public BreathHunter(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
