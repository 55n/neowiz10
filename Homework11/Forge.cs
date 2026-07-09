using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{ 
    class Forge
    {
        public int viewCraftableItems()
        {
            List<ItemType> itemList = new ItemData().getAllItemData();
            List<int[]> indexMap = new List<int[]>();
            int index = 0;

            Console.WriteLine();
            Console.WriteLine("##########################     제작 아이템 목록     #########################");
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].isCraftable)
                {
                    int[] indexPair = new int[2];
                    indexPair[0] = index;
                    indexPair[1] = i;
                    indexMap.Add(indexPair);
                    Console.WriteLine($"{index}번:   {itemList[i].name}     |    {itemList[i].description}");
                    index++;
                }
            }
            Console.WriteLine("############################################################################");

            // ==========================================   컨트롤러 영역  ===============================================

            Console.Write("어떤 아이템을 제작할까? : ");
            int playerAnswer = int.Parse(Console.ReadLine());
            Console.WriteLine();

            for (int i = 0; i < indexMap.Count; i++)
            {
                if (indexMap[i][0] == playerAnswer)
                {
                    return indexMap[i][1];
                }
            }

            return -1;
        }

        public Item craftItem(int itemId) // 나중에 아이템을 제작하는데 필요한 재료를 매개변수로 받아야 함
        {
            ItemFactory itemFactory = new ItemFactory();

            Item item = itemFactory.getItem(itemId);

            if (item.isCraftable)
            {
                Console.WriteLine($"{item.name} 을(를) 제작했다");
                return item;
            }
            else
            {
                Console.WriteLine($"{item.name} 은(는) 제작할 수 없는 아이템이다");
                return null;
            }
        }

        public void dismantleItem(Item item) // 나중에 아이템을 분해하고 받는 재료를 리턴해야 함. isDismantlable 필드를 추가할 것
        {
            Console.WriteLine("아이템이 분해되었습니다");
        }

        public int enhanceEquipment(IItemUpgradeable item) // 나중에 강화하는데 필요한 재료를 매개변수로 받아야 함
        {
            Item i = (Item)item;

            if (i.maxUpgradeCount <= i.upgradeCount)
            {
                return 0;
            }

            bool upgradeResult = item.upgrade();

            if (upgradeResult)
            {
                Console.WriteLine($"{i.upgradeCount} 번째 강화를 성공했습니다 (남은 강화 횟수: {i.maxUpgradeCount - i.upgradeCount}");
                return i.upgradeCount;
            }
            else
            {
                return -1;
            }
        }

        
    }
}
