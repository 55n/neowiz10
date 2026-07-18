using System;

namespace Darkness
{
    public class Trade
    {
        public Inventory FirstInventory { get; private set; }
        public Inventory SecondInventory { get; private set; }

        public Trade(Inventory firstInventory, Inventory secondInventory)
        {
            FirstInventory = firstInventory;
            SecondInventory = secondInventory;
        }

        public void Exchange(ItemStack itemStack)
        {
            Console.Write("Trade Exchange");
        }

        public int Loot(ItemStack itemStack)
        {
            if (itemStack == null)
            {
                return 0;
            }

            Item item = itemStack.Item;
            int removedCount = FirstInventory.Discard(itemStack);
            return SecondInventory.Store(new ItemStack(item, removedCount));
        }
    }
}
