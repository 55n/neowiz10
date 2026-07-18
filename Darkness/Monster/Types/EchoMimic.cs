using System;
using System.Collections.Generic;

namespace Darkness
{
    public class EchoMimic : Monster
    {
        public const string TypeId = "echo_mimic";

        public EchoMimic(MonsterType type, Inventory inventory, List<ActiveEffect> effects)
            : base(type, inventory, effects, TypeId)
        {
        }
    }
}
