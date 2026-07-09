using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 인벤토리 키우기 만들기
// 1. 아이템 만들기
// 2. 아이템 삭제하기
// 3. 인벤토리 비우기
// 4. 아이템 강화하기
// 5. 인벤토리 확장(기본 4칸)

// 1.아이템 만들기 기능 -> 랜덤으로 해도 되고 or 특정 아이템만 생성할 수 있도록 하셔도 됩니다. 
// 2. 아이템 삭제하기 -> 특정 아이템 삭제
// 3. 인벤토리 비우기 -> 전부 아이템 삭제
// 4. 아이템 강화하기 -> 일정확율로 터질수 있음 ㅋㅋㅋ
// 5. 인벤토리 강화하기 -> 강화하면 4칸씩 늘어나도록 합시다.


namespace Homework11
{
    class Program
    {
        public void GameLoop()
        {
            Console.WriteLine();
            Console.WriteLine("############# 인벤토리 키우기 #############");
            Console.WriteLine();
            Inventory inventory = new Inventory(4);
            Forge forge = new Forge();

            string playerAnswer;

            while(true)
            {
                inventory.veiwInventory();
                Console.Write("행동 선택) | 1. 아이템 제작 | 2. 인벤토리 강화 |    : ");
                playerAnswer = Console.ReadLine();
                
                if(playerAnswer == "1")
                {
                    forge.viewCraftableItems();
                    Console.Write("어떤 아이템을 제작할까? : ");
                    playerAnswer = Console.ReadLine();
                    
                    
                }
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.GameLoop();
        }
    }
}
