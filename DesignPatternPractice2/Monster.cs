using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice2
{
    abstract class Monster
    {
        public abstract void Attack();
    }

    class Goblin : Monster
    {
        public override void Attack()
        {
            Console.WriteLine("고블린 공격!");
        }
    }

    class Skeleton : Monster
    {
        public override void Attack()
        {
            Console.WriteLine("해골 공격!");
        }
    }

    class Dragon : Monster
    {
        public override void Attack()
        {
            Console.WriteLine("드래곤 브레스!");
        }
    }
}
