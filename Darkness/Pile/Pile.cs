namespace Darkness
{
    public class Pile : ISlotContent
    {
        public string Name { get { return "물건 더미"; } }
        public Inventory Inventory { get; private set; }

        public Pile(Inventory inventory)
        {
            Inventory = inventory;
        }

        public SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (context.Action != PlayerActionType.Search)
            {
                result.Messages.Add(
                    ExplorationMessages.NoResponse());
                return result;
            }

            if (Inventory.ItemStacks.Count == 0)
            {
                result.Messages.Add(
                    ExplorationMessages.PileEmpty());
                return result;
            }

            bool inventoryFull = false;
            foreach (ItemStack itemStack in
                     Inventory.ItemStacks.ToArray())
            {
                int itemCount = itemStack.Count;
                int transferred = Inventory.TransferTo(
                    itemStack,
                    itemCount,
                    context.Actor.Inventory);
                if (transferred > 0)
                {
                    result.Messages.Add(
                        InventoryMessages.ItemObtained(
                            itemStack.Item.Type.Name));
                }

                if (transferred < itemCount)
                {
                    inventoryFull = true;
                }
            }

            if (inventoryFull)
            {
                result.Messages.Add(
                    InventoryMessages.InventoryFull());
            }

            return result;
        }
    }
}
