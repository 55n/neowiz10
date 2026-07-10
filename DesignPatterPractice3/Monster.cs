using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterPractice3
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

    class Orc : Monster
    {
        public override void Attack()
        {
            Console.WriteLine("오크 공격!");
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
