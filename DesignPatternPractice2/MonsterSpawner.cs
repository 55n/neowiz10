using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternPractice2
{
    class MonsterSpawner
    {
        public void SpawnMonster()
        {
            Console.WriteLine("어떤 몬스터를 생성할까요?");
            Console.WriteLine("1. Goblin");
            Console.WriteLine("2. Skeleton");
            Console.WriteLine("3. Dragon");

            string input = Console.ReadLine();

            Monster monster = null;

            switch (input)
            {
                case "1":
                    monster = new Goblin();
                    break;

                case "2":
                    monster = new Skeleton();
                    break;

                case "3":
                    monster = new Dragon();
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    return;
            }

            // 생성 이후의 공통 작업
            Console.WriteLine("스탯 초기화");
            Console.WriteLine("AI 등록");
            Console.WriteLine("맵에 배치");

            Console.WriteLine($"{monster.GetType().Name} 생성 완료");
        }
    }
}
