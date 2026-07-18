using System;
using System.Collections.Generic;

namespace Darkness
{
    public class LostGoblin : Monster
    {
        public const string TypeId = "lost_goblin";

        public LostGoblin(
            MonsterType type,
            Inventory inventory,
            List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
