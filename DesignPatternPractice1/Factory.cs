using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice1
{
    interface IMonsterFactory
    {
        IMeleeMonster CreateMeleeMonster();
        IRangedMonster CreateRangedMonster();
        ICasterMonster CreateCasterMonster();
    }

    class GoblinFactory : IMonsterFactory
    {
        public IMeleeMonster CreateMeleeMonster()
        {
            return new Goblin();
        }

        public IRangedMonster CreateRangedMonster()
        {
            return new GoblinArcher();
        }

        public ICasterMonster CreateCasterMonster()
        {
            return new GoblinShaman();
        }
    }

    class UndeadFactory : IMonsterFactory
    {
        public IMeleeMonster CreateMeleeMonster()
        {
            return new Skeleton();
        }

        public IRangedMonster CreateRangedMonster()
        {
            return new Zombie();
        }

        public ICasterMonster CreateCasterMonster()
        {
            return new Lich();
        }
    }

    class DragonFactory : IMonsterFactory
    {
        public IMeleeMonster CreateMeleeMonster()
        {
            return new Dragon();
        }

        public IRangedMonster CreateRangedMonster()
        {
            return new Drake();
        }

        public ICasterMonster CreateCasterMonster()
        {
            return new Wyvern();
        }
    }
}
