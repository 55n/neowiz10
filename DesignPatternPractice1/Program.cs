using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("던전의 종족을 선택하세요.");
            Console.WriteLine();
            Console.WriteLine("| 1. 고블린 | 2. 언데드 | 3. 드래곤 |");

            string input = Console.ReadLine();

            IMonsterFactory factory = null;

            switch (input)
            {
                case "1":
                    factory = new GoblinFactory();
                    break;

                case "2":
                    factory = new UndeadFactory();
                    break;

                case "3":
                    factory = new DragonFactory();
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return;
            }

            DungeonGenerator generator = new DungeonGenerator();

            generator.GenerateDungeon(factory);
        }
    }
}
