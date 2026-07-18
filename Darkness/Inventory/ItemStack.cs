using System;

namespace Darkness
{
    public class ItemStack
    {
        public Item Item { get; private set; }
        public int Count { get; private set; }

        public ItemStack(Item item, int count)
        {
            Item = item;
            Count = count;
        }

        public int Add(int count)
        {
            int available = Item.Type.MaxStackCount - Count;
            int added = Math.Min(available, Math.Max(0, count));
            Count += added;
            return added;
        }

        public int Remove(int count)
        {
            int removed = Math.Min(Count, Math.Max(0, count));
            Count -= removed;
            return removed;
        }
    }
}
