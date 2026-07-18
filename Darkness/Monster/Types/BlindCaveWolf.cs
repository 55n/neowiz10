using System;
using System.Collections.Generic;

namespace Darkness
{
    public class BlindCaveWolf : Monster
    {
        public const string TypeId = "blind_cave_wolf";

        public BlindCaveWolf(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
