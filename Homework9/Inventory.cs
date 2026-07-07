using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework9
{
    class Inventory
    {
        private ItemStack[] inventory;
        private int inventorySize;

        public Inventory(int inventorySize)
        {
            this.inventorySize = inventorySize;
            this.inventory = new ItemStack[this.inventorySize];
        }

        public bool load(Item item)
        {
            for (int i = 0; i < this.inventory.Length; i++)
            {
                // 1. 있는지 확인
                if (this.inventory[i] == null) // 없으면 인벤토리에 스택을 새로 생성하여 아이템 적재
                {
                    this.inventory[i] = new ItemStack(item);
                    this.inventory[i].itemStack[0] = item;
                    return true;
                }
                // 2. 있으면 같은 타입인지 확인
                else if (this.inventory[i].itemStack[0].itemType.typeId == item.itemType.typeId)
                {
                    // 3. 타입이 같으면 슬롯에 빈 공간이 있는지 확인
                    for (int j = 0; j < this.inventory[i].itemStack.Length; j++)
                    {
                        if (this.inventory[i].itemStack[j] == null) // 빈 공간이 있으면 적재
                        {
                            this.inventory[i].itemStack[j] = item;
                            return true;
                        }
                    }
                }
            }
            // 4. 스택에 빈 공간도 없고 남는 슬롯도 없으면 적재 불가
            return false;
        }

        public Item[] pick(int index, int amount)
        {
            int lastIndex = 0;

            if (this.inventory[index] == null) // 빈 슬롯을 고르면 안 됨
            {
                return null;
            }

            if (amount > this.inventory[index].itemStack.Length) // 맥스 스택 보다 많은걸 요구해도 안 됨
            {
                return null;
            }

            for (int i = 0; i < this.inventory[index].itemStack.Length; i++)
            {
                if (this.inventory[index].itemStack[i] == null)
                {
                    lastIndex = i - 1;

                    if (amount > i)
                    {
                        return null;
                    }

                    break;
                }

                if (i == this.inventory[index].itemStack.Length - 1)
                {
                    lastIndex = i;

                    if (amount > this.inventory[index].itemStack.Length)
                    {
                        return null;
                    }
                }
            }

            // 위 조건을 통과한 경우 아이템 반환
            Item[] items = new Item[amount];
            for (int i = 0; i < amount; i++)
            {
                items[i] = this.inventory[index].itemStack[lastIndex - i];
                this.inventory[index].itemStack[lastIndex - i] = null;

                if (this.inventory[index].itemStack[0] == null)
                {
                    this.inventory[index] = null;
                }
            }
            return items;
        }

        public void veiwInventory()
        {
            Console.WriteLine($"########### 인벤토리 ({this.inventorySize} 칸) ###########");
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if (this.inventory[i] == null)
                {
                    Console.WriteLine($"{i}번 슬롯     비어있음");
                }
                else
                {
                    int count;
                    for (count = 0; count < this.inventory[i].itemStack.Length; count++)
                    {
                        if (this.inventory[i].itemStack[count] == null) break;
                    }

                    Console.WriteLine($"{i}번 슬롯     {this.inventory[i].itemStack[0].itemType.name} : {count} 개  {this.inventory[i].itemStack[0].itemType.price}G");
                }
            }
            Console.WriteLine("########################################");

        }
    }
}
