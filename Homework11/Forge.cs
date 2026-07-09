using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{ 
    class Forge
    {
        public void viewCraftableItems()
        {
            ItemData idb = new ItemData();
            int index = 0;
            Console.WriteLine();
            Console.WriteLine("################ 제작 아이템 목록 ###############");
            foreach (ItemType itemType in idb.getAllItemData())
            {
                if (itemType.isCraftable)
                {
                    Console.WriteLine($"{index}번 아이템    {itemType.name}     |    {itemType.description}");
                }
            }
            Console.WriteLine("#######################################");
            Console.WriteLine();
        }

        public Item craftItem(int craftableItemIndex) // 나중에 아이템을 제작하는데 필요한 재료를 매개변수로 받아야 함
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

        public void enhanceEquipment(IItemUpgradeable item) // 나중에 강화하는데 필요한 재료를 매개변수로 받아야 함
        {
            item.upgrade();
        }

        
    }
}
