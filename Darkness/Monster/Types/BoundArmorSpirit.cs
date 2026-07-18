using System;
using System.Collections.Generic;

namespace Darkness
{
    public class BoundArmorSpirit : Monster
    {
        public const string TypeId = "bound_armor_spirit";

        public BoundArmorSpirit(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
