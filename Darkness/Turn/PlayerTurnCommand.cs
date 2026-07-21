namespace Darkness
{
    public class PlayerTurnCommand
    {
        public PlayerActionType Action { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public ItemStack Item { get; private set; }
        public SkillUseContext SkillUse { get; private set; }
        public EquipmentSlot? ItemSourceEquipmentSlot { get; private set; }
        public bool ConsumesTurn { get; private set; }
        public bool AnnouncesAction { get; private set; }

        public PlayerTurnCommand(
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item,
            bool consumesTurn)
            : this(action, targetSlot, item, null, null, consumesTurn)
        {
        }

        public PlayerTurnCommand(
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item,
            SkillUseContext skillUse,
            bool consumesTurn)
            : this(
                action,
                targetSlot,
                item,
                skillUse,
                null,
                consumesTurn)
        {
        }

        public PlayerTurnCommand(
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item,
            SkillUseContext skillUse,
            EquipmentSlot? itemSourceEquipmentSlot,
            bool consumesTurn,
            bool announcesAction = true)
        {
            Action = action;
            TargetSlot = targetSlot;
            Item = item;
            SkillUse = skillUse;
            ItemSourceEquipmentSlot = itemSourceEquipmentSlot;
            ConsumesTurn = consumesTurn;
            AnnouncesAction = announcesAction;
        }
    }
}
