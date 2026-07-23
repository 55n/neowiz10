namespace Darkness
{
    public class Room0DeveloperSwordBehavior : IRoomTurnBehavior
    {
        private const int TargetSlotIndex = 0;
        private const int RequiredSearchCount = 3;
        private const string DeveloperSwordItemId = "developer_sword";

        private int searchCount;
        private bool swordGranted;

        public SlotInteractionResult Act(
            Room room,
            Hero hero,
            PlayerActionContext playerAction)
        {
            SlotInteractionResult result =
                new SlotInteractionResult();
            if (swordGranted || room == null || hero == null ||
                playerAction == null ||
                playerAction.Action != PlayerActionType.Search ||
                room.Slots.Count <= TargetSlotIndex ||
                !ReferenceEquals(
                    playerAction.TargetSlot,
                    room.Slots[TargetSlotIndex]))
            {
                return result;
            }

            searchCount++;
            if (searchCount < RequiredSearchCount)
            {
                return result;
            }

            ItemType swordType =
                new ItemData().ItemTypes[DeveloperSwordItemId];
            ItemStack sword = new ItemStack(
                new Item(swordType),
                1);
            int remaining = hero.Inventory.Store(sword);

            result.Messages.Add(
                "같은 자리를 세 번째로 더듬자 바닥의 갈라진 틈에서 낯선 검이 모습을 드러냈다.");
            if (remaining == 0)
            {
                result.Messages.Add(
                    InventoryMessages.ItemObtained(swordType.Name));
            }
            else
            {
                room.Slots[TargetSlotIndex].GroundInventory.Store(sword);
                result.Messages.Add(
                    "소지품이 가득 차 개발자의 검을 그 자리에 내려놓았다.");
            }

            swordGranted = true;
            return result;
        }
    }
}
