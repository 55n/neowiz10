using System;
using System.Collections.Generic;

namespace Darkness
{
    public class DoorClinger : Monster
    {
        public const string TypeId = "door_clinger";

        public DoorClinger(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
