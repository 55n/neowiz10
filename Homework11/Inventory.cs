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
                if (this.inventory.Count <= i) // 인벤토리에 스택을 새로 생성하여 아이템 적재
                {
                    this.inventory.Add(new ItemStack(item));
                    this.inventory[i].pushItemToStack(item);
                    return true;
                }
                else if (this.inventory[i].itemId == item.itemId) // 같은 아이템의 스택이 있다면 슬롯에 빈 공간이 있는지 확인
                {
                    if (this.inventory[i].pushItemToStack(item))
                    {
                        return true;
                    }
                }
            }
            // 4. 스택에 빈 공간도 없고 남는 슬롯도 없으면 적재 불가
            return false;
        }

        public void makeEmpty()
        {
            this.inventory.Clear();
        }

        public Item[] pick(int index, int amount)
        {
            if (this.inventory.Count <= index) return null; // 빈 슬롯을 고르면 안 됨

            //if (this.inventory[index].maxAmount < amount) return null; // 스택 최대 크기보다 더 많은 양을 집으려 하면 안 됨

            if (this.inventory[index].getLastIndex() + 1 < amount) return null; // 스택 현재 카운트보다 더 많은 양을 집으려 하면 안 됨

            Item[] items = new Item[amount];

            for (int i = 0; i < amount; i++)
            {
                items[i] = this.inventory[index].popItemFromStack();
            }

            if (this.inventory.Count == amount)
            {
                this.inventory.RemoveAt(index);
            }

            return items;
        }

        public Item[] pick(int index) // 인벤토리 한 칸의 모든 아이템 가져옴
        {
            if (this.inventory.Count <= index) return null; // 빈 슬롯을 고르면 안 됨

            Item[] items = new Item[this.inventory[index].getLastIndex() + 1];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = this.inventory[index].popItemFromStack();
                this.inventory.RemoveAt(index);
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
                if (this.inventory.Count <= i)
                {
                    Console.WriteLine($"{i}번 슬롯     비어있음");
                }
                else
                {
                    Console.WriteLine($"{i}번 슬롯     {this.inventory[i].name} : {this.inventory[i].getLastIndex() + 1} 개  {this.inventory[i].price}G");
                }
            }
            Console.WriteLine("#######################################");
        }
    }
}
