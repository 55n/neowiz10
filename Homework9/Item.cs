using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework9
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
        public ItemType itemType { get; }
        public int itemId { get; private set; }
        public string itemName { get; private set; }
        public string itemDescription { get; private set; }
        public int itemMaxStack { get; private set; }
        public int itemPrice { get; private set; }

        public Item(int typeId)
        {
            this.itemType = new ItemData().itemType[typeId];

            this.itemName = itemType.name;
            this.itemDescription = itemType.description;
            this.itemMaxStack = itemType.maxAmount;
            this.itemPrice = itemType.price;

        }
    }

    class Gold : Item
    {

    }

    class ItemStack
    {
        public Item[] itemStack;

        public ItemStack(Item item)
        {
            itemStack = new Item[item.itemMaxStack];
        }
    }
}
