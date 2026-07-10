using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice
{
    abstract class Monster
    {
        public abstract void Attack();
    }

    // 그린스킨
    class Goblin : Monster
    {
        public override void Attack() { }
    }

    class GoblinArcher : Monster
    {
        public override void Attack() { }
    }

    class GoblinShaman : Monster
    {
        public override void Attack() { }
    }

    // 언데드
    class Skeleton : Monster
    {
        public override void Attack() { }
    }

    class Zombie : Monster
    {
        public override void Attack() { }
    }

    class Lich : Monster
    {
        public override void Attack() { }
    }


    // 드래곤
    class Dragon : Monster
    {
        public override void Attack() { }
    }

    class Drake : Monster
    {
        public override void Attack() { }
    }

    class Wyvern : Monster
    {
        public override void Attack() { }
    }
}
