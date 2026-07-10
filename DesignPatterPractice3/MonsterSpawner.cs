using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterPractice3
{
    abstract class MonsterSpawner
    {
        // Factory Method
        protected abstract Monster CreateMonster();

        // 공통 로직
        public void SpawnMonster()
        {
            Monster monster = CreateMonster();

            Console.WriteLine("스탯 초기화");
            Console.WriteLine("AI 등록");
            Console.WriteLine("맵에 배치");

            Console.WriteLine($"{monster.GetType().Name} 생성 완료");
        }
    }
}
