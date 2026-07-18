using System;
using System.Collections.Generic;

namespace Darkness
{
    public class SwampWaterSpirit : Monster
    {
        public const string TypeId = "swamp_water_spirit";

        public SwampWaterSpirit(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
