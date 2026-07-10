using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice
{
    class DungeonGenerator
    {
        public void GenerateDungeon()
        {
            Console.WriteLine("던전의 종족을 선택하세요.");
            Console.WriteLine();
            Console.WriteLine("| 1. 고블린 | 2. 언데드 | 3. 드래곤 |");
            Console.Write("선택 : ");

            string input = Console.ReadLine();

            Monster melee = null;
            Monster ranged = null;
            Monster caster = null;

            string race = "";

            switch (input)
            {
                case "1":
                    race = "고블린";

                    melee = new Goblin();
                    ranged = new GoblinArcher();
                    caster = new GoblinShaman();

                    break;

                case "2":
                    race = "언데드";

                    melee = new Skeleton();
                    ranged = new Zombie();
                    caster = new Lich();

                    break;

                case "3":
                    race = "드래곤";

                    melee = new Dragon();
                    ranged = new Drake();
                    caster = new Wyvern();

                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return;
            }

            Console.WriteLine();
            Console.WriteLine($"{race} 던전 생성 완료!");
            Console.WriteLine();

            Console.WriteLine("배치된 몬스터");

            Console.WriteLine($"- {melee.GetType().Name}");
            Console.WriteLine($"- {ranged.GetType().Name}");
            Console.WriteLine($"- {caster.GetType().Name}");
        }
    }
}
