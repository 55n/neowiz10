using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework9
{
    class Trade
    {
        public Inventory inventory1 { get; private set; }
        public Inventory inventory2 { get; private set; }

        public Trade(Inventory sellerInventory, Inventory buyerInventory)
        {
            this.inventory1 = sellerInventory;
            this.inventory2 = buyerInventory;
        }

        public bool tradeItems(int index, int amount)
        {
            Item[] items = this.inventory1.pick(index, amount);

            int price = items[0].itemType.price * amount;

            Item[] gold = this.inventory2.pick(0, price);

            if (gold == null)
            {
                Console.WriteLine($"돈이 부족하다");

                for (int i = 0; i < items.Length; i++)
                {
                    inventory1.load(items[i]);
                }

                return false;
            }

            for (int i = 0; i < gold.Length; i++)
            {
                inventory1.load(gold[i]);
            }

            for (int i = 0; i < items.Length; i++)
            {
                inventory2.load(items[i]);
            }

            return true;

        }
    }
}
