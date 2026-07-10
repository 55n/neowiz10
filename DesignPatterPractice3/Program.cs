using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterPractice3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("어떤 몬스터를 생성할까요?");
            Console.WriteLine("1. Goblin");
            Console.WriteLine("2. Skeleton");
            Console.WriteLine("3. Dragon");

            string input = Console.ReadLine();

            MonsterSpawner spawner = null;

            switch (input)
            {
                case "1":
                    spawner = new GoblinSpawner();
                    break;

                case "2":
                    spawner = new SkeletonSpawner();
                    break;

                case "3":
                    spawner = new DragonSpawner();
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return;
            }

            spawner.SpawnMonster();
        }
    }
}
