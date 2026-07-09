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
            Console.WriteLine("###########################################");
            Console.WriteLine("###########################################");
            Console.WriteLine("############# 인벤토리 키우기 #############");
            Console.WriteLine("###########################################");
            Console.WriteLine("###########################################");
            Console.WriteLine();

            Inventory inventory = new Inventory(4);
            Forge forge = new Forge();

            string playerAnswer;
            string systemMessage = "";

            while(true)
            {
                inventory.veiwInventory();
                Console.WriteLine(systemMessage);
                Console.WriteLine();
                Console.WriteLine("| 1. 아이템 제작 | 2. 아이템 강화 | 3. 인벤토리 강화 | 4. 아이템 제거 | 5. 인벤토리 비우기 | 6. 나가기 |");
                Console.Write("행동을 선택하세요: ");
                playerAnswer = Console.ReadLine();

                systemMessage = "";

                if (playerAnswer == "1")
                {
                    int itemId = forge.viewCraftableItems();
                    Item item = forge.craftItem(itemId);
                    if (item != null)
                    {
                        bool isLoaded = inventory.load(item);
                        if (!isLoaded) systemMessage = "인벤토리가 꽉 찼습니다";
                    }

                    Console.Clear();
                }
                else if (playerAnswer == "2")
                {
                    Console.WriteLine("어떤 아이템을 강화하시겠습니까?");
                    Console.Write("인벤토리 번호를 입력하세요: ");
                    int inventorySlotNumber = int.Parse(Console.ReadLine());
                    Item[] items = inventory.pick(inventorySlotNumber);
                    if (items != null)
                    {
                        if (items[0].maxUpgradeCount > items[0].upgradeCount)
                        {
                            Console.WriteLine($"{items[0].name}을 강화합니다 (성공확률: {items[0].upgradeProbability}%)");
                            Console.WriteLine("| 1. 확인 | 2. 취소 |");
                            Console.Write("진행하시겠습니까? : ");
                            string playerConfirm = Console.ReadLine();

                            if (playerConfirm == "1")
                            {
                                IItemUpgradeable upgradeableItem = (IItemUpgradeable)items[0];
                                int enhancementResult = forge.enhanceEquipment(upgradeableItem);

                                if (enhancementResult == -1)
                                {
                                    systemMessage = "강화 실패...";
                                }
                                else
                                {
                                    inventory.load((Item)upgradeableItem);
                                }
                            }
                            else
                            {
                                systemMessage = "강화를 취소했습니다";
                            }
                        }
                        else
                        {
                            inventory.load(items[0]);
                            systemMessage = "더 이상 강화할 수 없습니다";
                        }

                    }
                    else
                    {
                        systemMessage = "해당 아이템을 찾을 수 없습니다";
                    }
                    Console.Clear();
                }
                else if (playerAnswer == "3")
                {
                    Console.WriteLine("인벤토리를 강화하시겠습니까?");
                    Console.Write("예/아니요: ");
                    string yesOrNo = Console.ReadLine();

                    if (yesOrNo == "예")
                    {
                        inventory.upgrade(4);
                    }

                    Console.Clear();
                }
                else if (playerAnswer == "4")
                {
                    Console.WriteLine("삭제할 아이템의 인덱스를 고르세요: ");
                    Console.Write("인덱스: ");
                    int deleteIndex = int.Parse(Console.ReadLine());

                    Item[] items = inventory.pick(deleteIndex);

                    if (items == null)
                    {
                        systemMessage = "삭제할 수 없습니다";
                    }
                    else
                    {
                        systemMessage = "삭제되었습니다";
                    }
                    Console.Clear();
                }
                else if (playerAnswer == "5")
                {
                    inventory.makeEmpty();
                    Console.Clear();
                }
                else if (playerAnswer == "6")
                {
                    break;
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
