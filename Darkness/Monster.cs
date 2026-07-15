using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    class Monster
    {
        public class MonsterType
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int MaxHp { get; set; }
            public int Attack { get; set; }
            public int Defense { get; set; }

            public bool IsAggressive { get; set; }

            public List<string> Clues { get; set; }
            public string EncounterText { get; set; }
            public string RevealedText { get; set; }

            public int ExpReward { get; set; }
            public int MagicStoneReward { get; set; }
            public List<Item> DropItemIds { get; set; }
        }

        public class MonsterData
        {

        }

    }
}
