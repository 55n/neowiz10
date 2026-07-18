using System;
using System.Collections.Generic;

namespace Darkness
{
    public class VibrationWorm : Monster
    {
        public const string TypeId = "vibration_worm";

        public VibrationWorm(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
