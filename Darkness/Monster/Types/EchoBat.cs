using System;
using System.Collections.Generic;

namespace Darkness
{
    public class EchoBat : Monster
    {
        public const string TypeId = "echo_bat";

        public EchoBat(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
