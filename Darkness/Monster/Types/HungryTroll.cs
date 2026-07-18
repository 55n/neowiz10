using System;
using System.Collections.Generic;

namespace Darkness
{
    public class HungryTroll : Monster
    {
        public const string TypeId = "hungry_troll";

        public HungryTroll(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
