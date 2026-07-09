using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{
    enum ItemClass
    {
        GOLD, DISPOSABLE, EQUIPMENT
    }

    class ItemType
    {
        public int itemId { get; }
        public ItemClass itemClass { get; }
        public string name { get; }
        public string description { get; }
        public int maxAmount { get; }
        public int price { get; }
        public bool isCraftable { get; }

        public ItemType(int itemId, ItemClass itemClass, string name, string description, int maxAmount, int price, bool isCraftable)
        {
            this.itemId = itemId;
            this.itemClass = itemClass;
            this.name = name;
            this.description = description;
            this.maxAmount = maxAmount;
            this.price = price;
            this.isCraftable = isCraftable;
        }
    }
    
    class ItemData
    {
        private List<ItemType> itemTypes;

        public ItemData()
        {
            itemTypes.Add(new ItemType(0, ItemClass.GOLD, "골드", "화폐", 9999, 1, false));
            itemTypes.Add(new ItemType(1, ItemClass.DISPOSABLE, "HP 포션", "HP를 10% 회복시킨다", 10, 10, false));
            itemTypes.Add(new ItemType(2, ItemClass.DISPOSABLE, "MP 포션", "MP를 1 회복시킨다", 10, 10, false));
            itemTypes.Add(new ItemType(3, ItemClass.EQUIPMENT, "기본 칼", "기본으로 주어지는 전투용 칼", 1, 12, true));
            itemTypes.Add(new ItemType(4, ItemClass.EQUIPMENT, "벌목용 도끼", "나무를 베는데 쓰이는 도끼", 1, 12, true));
            itemTypes.Add(new ItemType(5, ItemClass.EQUIPMENT, "좋은 칼", "좋아 보이는 칼", 1, 20, true));
            itemTypes.Add(new ItemType(6, ItemClass.EQUIPMENT, "기본 투구", "기본으로 주어지는 투구", 1, 13, true));
            itemTypes.Add(new ItemType(7, ItemClass.EQUIPMENT, "기본 장갑", "기본으로 주어지는 장갑", 1, 13, true));
            itemTypes.Add(new ItemType(8, ItemClass.EQUIPMENT, "기본 신발", "기본으로 주어지는 신발", 1, 13, true));
        }

        public ItemType getItemData(int index)
        {
            return itemTypes[index];
        }

        public List<ItemType> getAllItemData()
        {
            return itemTypes;
        }
    }

    abstract class Item
    {
        public int itemId { get; protected set; }
        public ItemClass itemClass { get; protected set; }
        public string name { get; protected set; }
        public string description { get; protected set; }
        public int maxAmount { get; protected set; }
        public int price { get; protected set; }
        public bool isCraftable { get; protected set; }
    }

    class Gold : Item
    {
        public Gold()
        {
            ItemType i = new ItemData().getItemData(0);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }
    }

    class HpPotion : Item
    {
        public HpPotion()
        {
            ItemType i = new ItemData().getItemData(1);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }
    }

    class MpPotion : Item
    {
        public MpPotion()
        {
            ItemType i = new ItemData().getItemData(2);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }
    }

    class BasicSword : Item, IItemUpgradeable
    {
        public BasicSword()
        {
            ItemType i = new ItemData().getItemData(3);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }

        public void upgrade()
        {
            this.name += "+";
            this.price += 10;
        }
    }
    class FellingAxe : Item, IItemUpgradeable
    {
        public FellingAxe()
        {
            ItemType i = new ItemData().getItemData(4);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }

        public void upgrade()
        {
            this.name += "+";
            this.price += 10;
        }
    }
    class GoodSword : Item, IItemUpgradeable
    {
        public GoodSword()
        {
            ItemType i = new ItemData().getItemData(5);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }

        public void upgrade()
        {
            this.name += "+";
            this.price += 10;
        }
    }
    class BasicHelm : Item, IItemUpgradeable
    {
        public BasicHelm()
        {
            ItemType i = new ItemData().getItemData(6);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }

        public void upgrade()
        {
            this.name += "+";
            this.price += 10;
        }
    }
    class BasicGloves : Item, IItemUpgradeable
    {
        public BasicGloves()
        {
            ItemType i = new ItemData().getItemData(7);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }

        public void upgrade()
        {
            this.name += "+";
            this.price += 10;
        }
    }
    class BasicBoots : Item, IItemUpgradeable
    {
        public BasicBoots()
        {
            ItemType i = new ItemData().getItemData(8);
            this.itemId = i.itemId;
            this.itemClass = i.itemClass;
            this.name = i.name;
            this.description = i.description;
            this.maxAmount = i.maxAmount;
            this.price = i.price;
            this.isCraftable = i.isCraftable;
        }

        public void upgrade()
        {
            this.name += "+";
            this.price += 10;
        }
    }


    class ItemStack
    {
        private List<Item> itemStack;
        public int itemId { get; }
        public ItemClass itemClass { get; }
        public string name { get; }
        public int maxAmount { get; }
        public int price { get; }

        public ItemStack(Item item)
        {
            this.itemId = item.itemId;
            this.itemClass = item.itemClass;
            this.name = item.name;
            this.maxAmount = item.maxAmount;
            this.price = item.price;
            this.itemStack = new List<Item>();
        }

        public bool pushItemToStack(Item item)
        {
            int index = getLastIndex();

            if (index >= item.maxAmount) return false;

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
                itemStack.RemoveAt(index);
                return tmp;
            }
        }

        public int getLastIndex() // 스택에 아무 것도 없으면 -1 반환
        {
            int lastIndex = -1;

            if (this.itemStack[0] != null) lastIndex = this.itemStack.Count - 1; 

            return lastIndex;
        }
    }

    class ItemFactory 
    {
        private ItemData itemData;
        
        public ItemFactory()
        {
            this.itemData = new ItemData();
        }

        public Item getItem(int itemId) // 지금은 ItemType의 itemId와 ItemData의 itemId가 동일하지만 달라질 수도 있으니깐...
        {
            ItemType item = this.itemData.getItemData(itemId);

            switch (item.itemId)
            {
                case 0:
                    return new Gold();
                case 1:
                    return new HpPotion();
                case 2:
                    return new MpPotion();
                case 3:
                    return new BasicSword();
                case 4:
                    return new FellingAxe();
                case 5:
                    return new GoodSword();
                case 6:
                    return new BasicHelm();
                case 7:
                    return new BasicGloves();
                case 8:
                    return new BasicBoots();
                default:
                    return null;
            }
        }
    }
}
