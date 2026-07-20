namespace Darkness
{
    public class PlayerActionContext
    {
        public Hero Actor { get; private set; }
        public PlayerActionType Action { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public ItemStack Item { get; private set; }
        public SkillUseContext SkillUse { get; private set; }
        public EquipmentSlot? ItemSourceEquipmentSlot { get; private set; }

        public PlayerActionContext(
            Hero actor,
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item,
            SkillUseContext skillUse)
            : this(actor, action, targetSlot, item, skillUse, null)
        {
        }

        public PlayerActionContext(
            Hero actor,
            PlayerActionType action,
            RoomSlot targetSlot,
            ItemStack item,
            SkillUseContext skillUse,
            EquipmentSlot? itemSourceEquipmentSlot)
        {
            Actor = actor;
            Action = action;
            TargetSlot = targetSlot;
            Item = item;
            SkillUse = skillUse;
            ItemSourceEquipmentSlot = itemSourceEquipmentSlot;
        }
    }
}
