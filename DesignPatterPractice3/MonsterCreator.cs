using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterPractice3
{
    class GoblinSpawner : MonsterSpawner
    {
        protected override Monster CreateMonster()
        {
            return new Goblin();
        }
    }

    class SkeletonSpawner : MonsterSpawner
    {
        protected override Monster CreateMonster()
        {
            return new Orc();
        }
    }

    class DragonSpawner : MonsterSpawner
    {
        protected override Monster CreateMonster()
        {
            return new Dragon();
        }
    }
}
