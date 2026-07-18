using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Inventory
    {
        public List<ItemStack> ItemStacks { get; private set; }
        public int Capacity { get; private set; }

        public Inventory(int capacity)
        {
            Capacity = capacity;
            ItemStacks = new List<ItemStack>();
        }

        public int Store(ItemStack itemStack)
        {
            if (itemStack == null || itemStack.Item == null || itemStack.Count <= 0)
            {
                return 0;
            }

            int remaining = itemStack.Count;
            string itemId = itemStack.Item.Type.Id;

            foreach (ItemStack storedStack in ItemStacks)
            {
                if (storedStack.Item.Type.Id == itemId)
                {
                    remaining -= storedStack.Add(remaining);
                    if (remaining == 0)
                    {
                        return 0;
                    }
                }
            }

            while (remaining > 0 && ItemStacks.Count < Capacity)
            {
                int stackCount = Math.Min(remaining, itemStack.Item.Type.MaxStackCount);
                ItemStacks.Add(new ItemStack(itemStack.Item, stackCount));
                remaining -= stackCount;
            }

            return remaining;
        }

        public int Discard(ItemStack itemStack)
        {
            return Discard(itemStack, itemStack == null ? 0 : itemStack.Count);
        }

        public int Discard(ItemStack itemStack, int count)
        {
            if (itemStack == null || !ItemStacks.Contains(itemStack))
            {
                return 0;
            }

            int removed = itemStack.Remove(count);
            if (itemStack.Count == 0)
            {
                ItemStacks.Remove(itemStack);
            }

            return removed;
        }
    }
}
