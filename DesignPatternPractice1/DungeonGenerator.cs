using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice1
{
    class DungeonGenerator
    {
        public void GenerateDungeon(IMonsterFactory factory)
        {
            IMeleeMonster melee = factory.CreateMeleeMonster();
            IRangedMonster ranged = factory.CreateRangedMonster();
            ICasterMonster caster = factory.CreateCasterMonster();

            Console.WriteLine("던전 생성 완료!");
            Console.WriteLine();

            Console.WriteLine("배치된 몬스터");

            Console.WriteLine($"- {melee.GetType().Name}");
            Console.WriteLine($"- {ranged.GetType().Name}");
            Console.WriteLine($"- {caster.GetType().Name}");
        }
    }
}
