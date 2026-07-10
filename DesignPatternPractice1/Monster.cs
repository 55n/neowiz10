using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice1
{
    public interface IMeleeMonster
    {
        void Attack();
    }

    public interface IRangedMonster
    {
        void Attack();
    }

    public interface ICasterMonster
    {
        void Attack();
    }

    class Goblin : IMeleeMonster
    {
        public void Attack() { }
    }

    class GoblinArcher : IRangedMonster
    {
        public void Attack() { }
    }

    class GoblinShaman : ICasterMonster
    {
        public void Attack() { }
    }

    class Skeleton : IMeleeMonster
    {
        public void Attack() { }
    }

    class Zombie : IRangedMonster
    {
        public void Attack() { }
    }

    class Lich : ICasterMonster
    {
        public void Attack() { }
    }

    class Dragon : IMeleeMonster
    {
        public void Attack() { }
    }

    class Drake : IRangedMonster
    {
        public void Attack() { }
    }

    class Wyvern : ICasterMonster
    {
        public void Attack() { }
    }
}
