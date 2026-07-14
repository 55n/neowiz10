using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    class ItemType
    {
        public int typeId { get; }
        public string name { get; }
        public string description { get; }
        public int maxAmount { get; }
        public int price { get; }
        public ItemType(int typeId, string name, string description, int maxAmount, int price)
        {
            this.typeId = typeId;
            this.name = name;
            this.description = description;
            this.maxAmount = maxAmount;
            this.price = price;
        }
    }

    class ItemData
    {
        public ItemType[] itemType { get; }

        public ItemData()
        {
            itemType = new ItemType[3]
            {
                new ItemType(0, "골드", "화폐", 9999, 1),
                new ItemType(1, "HP 포션", "HP를 10% 회복시킨다", 10, 10),
                new ItemType(2, "MP 포션", "MP를 1 회복시킨다", 10, 10),
            };
        }
    }

    abstract class Item
    {
        public int itemType { get; protected set; }
        public string name { get; protected set; }
        public string description { get; protected set; }
        public int maxAmount { get; protected set; }
        public int price { get; protected set; }
        public abstract void use(Creature creature);
    }

    class Gold : Item
    {
        public Gold()
        {
            ItemType i = new ItemData().itemType[0];
            this.itemType = i.typeId;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
        }

        public override void use(Creature creature)
        {
            Console.WriteLine();
            Console.WriteLine("대체 뭘 기대하신 겁니까? 던지시게요??");
            Console.WriteLine();
        }
    }

    class HpPotion : Item
    {
        public HpPotion()
        {
            ItemType i = new ItemData().itemType[1];
            this.itemType = i.typeId;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
        }

        public override void use(Creature creature)
        {
            creature.heal(20);
        }
    }

    class MpPotion : Item
    {
        public MpPotion()
        {
            ItemType i = new ItemData().itemType[2];
            this.itemType = i.typeId;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
        }

        public override void use(Creature creature)
        {
            creature.awake(1);
        }
    }

    class ItemStack
    {
        private Item[] itemStack;
        public int itemType { get; }
        public string name { get; }
        public int maxAmount { get; }
        public int price { get; }

        public ItemStack(Item item)
        {
            this.itemType = item.itemType;
            this.name = item.name;
            this.maxAmount = item.maxAmount;
            this.price = item.price;
            this.itemStack = new Item[item.maxAmount];
        }

        public bool pushItemToStack(Item item)
        {
            int index = getLastIndex();
            if (index == this.itemStack.Length - 1)
            {
                return false;
            }

            if (index < 0)
            {
                itemStack[0] = item;
                return true;
            }
            else
            {
                itemStack[index + 1] = item;
                return true;
            }
        }

        public Item popItemFromStack()
        {
            int index = getLastIndex();

            if (index < 0)
            {
                return null;
            }
            else
            {
                Item tmp = itemStack[index];
                itemStack[index] = null;
                return tmp;
            }
        }

        public int getLastIndex()
        {
            int lastIndex = -1;

            // 스택에 아무 것도 없으면 -1 반환
            if (this.itemStack[0] == null) lastIndex = -1;
            else if (this.itemStack[this.itemStack.Length - 1] != null) lastIndex = this.itemStack.Length - 1;
            else
            {
                for (int i = 0; i < this.itemStack.Length; i++)
                {
                    if (this.itemStack[i] == null)
                    {
                        lastIndex = i - 1;
                        break;
                    }
                }
            }

            return lastIndex;
        }
    }
}
