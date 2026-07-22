using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Inventory
    {
        private string temporaryCapacityItemId;
        private int temporaryCapacityBonus;

        public List<ItemStack> ItemStacks { get; private set; }
        public int Capacity { get; private set; }

        public Inventory(int capacity)
        {
            Capacity = Math.Max(0, capacity);
            ItemStacks = new List<ItemStack>();
        }

        public void ExpandCapacity(int amount)
        {
            Capacity += Math.Max(0, amount);
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

        public int StoreWithTemporaryCapacity(
            ItemStack itemStack,
            string temporaryItemId)
        {
            int remaining = Store(itemStack);
            if (remaining == 0 || itemStack == null ||
                itemStack.Item == null ||
                itemStack.Item.Type.Id != temporaryItemId)
            {
                return remaining;
            }

            ExpandCapacity(1);
            temporaryCapacityItemId = temporaryItemId;
            temporaryCapacityBonus++;
            return Store(new ItemStack(itemStack.Item, remaining));
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

            string itemId = itemStack.Item.Type.Id;
            int removed = itemStack.Remove(count);
            if (itemStack.Count == 0)
            {
                ItemStacks.Remove(itemStack);
            }

            ReleaseTemporaryCapacityIfItemIsGone(itemId);

            return removed;
        }

        private void ReleaseTemporaryCapacityIfItemIsGone(
            string itemId)
        {
            if (temporaryCapacityBonus == 0 ||
                temporaryCapacityItemId != itemId ||
                ItemStacks.Exists(stack =>
                    stack.Item.Type.Id == itemId))
            {
                return;
            }

            Capacity = Math.Max(
                ItemStacks.Count,
                Capacity - temporaryCapacityBonus);
            temporaryCapacityItemId = null;
            temporaryCapacityBonus = 0;
        }

        public bool RemoveItem(Item item)
        {
            if (item == null)
            {
                return false;
            }

            ItemStack itemStack = ItemStacks.Find(
                stack => ReferenceEquals(stack.Item, item));
            return itemStack != null && Discard(itemStack, 1) == 1;
        }

        public int TransferTo(
            ItemStack itemStack,
            int count,
            Inventory target)
        {
            if (itemStack == null ||
                target == null ||
                ReferenceEquals(this, target) ||
                !ItemStacks.Contains(itemStack))
            {
                return 0;
            }

            int requested = Math.Min(itemStack.Count, Math.Max(0, count));
            if (requested == 0)
            {
                return 0;
            }

            int remaining = target.Store(
                new ItemStack(itemStack.Item, requested));
            int transferred = requested - remaining;
            if (transferred > 0)
            {
                Discard(itemStack, transferred);
            }

            return transferred;
        }

        public void TransferAllTo(Inventory target)
        {
            if (target == null || ReferenceEquals(this, target))
            {
                return;
            }

            foreach (ItemStack itemStack in ItemStacks.ToArray())
            {
                int originalCount = itemStack.Count;
                int remaining = target.Store(itemStack);
                int transferred = originalCount - remaining;
                if (transferred > 0)
                {
                    Discard(itemStack, transferred);
                }
            }
        }
    }
}
