using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{
    class Inventory : IInventoryUpgradeable
    {
        private List<ItemStack> inventory;
        private int inventorySize;

        public Inventory(int inventorySize)
        {
            this.inventorySize = inventorySize;
            this.inventory = new List<ItemStack>();
        }

        public bool load(Item item)
        {
            for (int i = 0; i < this.inventorySize; i++)
            {
                // 1. 있는지 확인
                if (this.inventory[i] == null) // 없으면 인벤토리에 스택을 새로 생성하여 아이템 적재
                {
                    this.inventory[i] = new ItemStack(item);
                    this.inventory[i].pushItemToStack(item);
                    return true;
                }
                // 2. 있으면 같은 타입인지 확인
                else if (this.inventory[i].itemId == item.itemId)
                {
                    // 3. 타입이 같으면 슬롯에 빈 공간이 있는지 확인
                    if (this.inventory[i].pushItemToStack(item))
                    {
                        return true;
                    }
                }
            }

            // 4. 스택에 빈 공간도 없고 남는 슬롯도 없으면 적재 불가
            return false;
        }

        public Item[] pick(int index, int amount)
        {
            if (this.inventory[index] == null) return null; // 빈 슬롯을 고르면 안 됨

            if (this.inventory[index].maxAmount < amount) return null; // 스택 최대 크기보다 더 많은 양을 집으려 하면 안 됨

            if (this.inventory[index].getLastIndex() + 1 < amount) return null; // 스택 현재 카운트보다 더 많은 양을 집으려 하면 안 됨

            Item[] items = new Item[amount];

            for (int i = 0; i < amount; i++)
            {
                items[i] = this.inventory[index].popItemFromStack();
            }

            return items;
        }

        public Item[] pick(int index) // 인벤토리 한 칸의 모든 아이템 가져옴
        {
            if (this.inventory[index] == null) return null; // 빈 슬롯을 고르면 안 됨

            int count = this.inventory[index].getLastIndex() + 1;

            Item[] items = new Item[count];

            for (int i = 0; i < count; i++)
            {
                items[i] = this.inventory[index].popItemFromStack();
            }

            return items;
        }

        public void upgrade(int size)
        {
            this.inventorySize += size;
        }

        public void veiwInventory()
        {
            Console.WriteLine($"########### 인벤토리 ({this.inventorySize} 칸) ###########");
            for (int i = 0; i < this.inventorySize; i++)
            {
                if (this.inventory[i] == null)
                {
                    Console.WriteLine($"{i}번 슬롯     비어있음");
                }
                else
                {
                    Console.WriteLine($"{i}번 슬롯     {this.inventory[i].name} : {this.inventory[i].getLastIndex() + 1} 개  {this.inventory[i].price}G");
                }
            }
            Console.WriteLine("########################################");
        }
    }
}
