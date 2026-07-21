using System.Collections.Generic;

namespace Darkness
{
    public class PoisonFogTrap : Trap, IRoomEffectSource
    {
        public object EffectSource { get { return this; } }
        public EffectOriginType EffectOriginType
        {
            get { return EffectOriginType.Trap; }
        }
        public string EffectOriginId { get { return Id; } }
        public bool IsRoomEffectActive { get { return IsActive; } }
        public IEnumerable<EffectApplication> RoomEffects
        {
            get { return Type.TriggerEffects; }
        }

        public PoisonFogTrap(TrapType type)
            : base(type)
        {
        }

        public override SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (context == null || !IsActive)
            {
                return result;
            }

            if (context.Action == PlayerActionType.Search)
            {
                result.Messages.Add(BodyMessages.PoisonTrapExamined());
                return result;
            }

            if (context.Action == PlayerActionType.Attack)
            {
                Item weapon = context.Actor.GetEquippedItem(
                    EquipmentSlot.Weapon);
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    this,
                    context.Actor.Attack,
                    context.Actor.Accuracy,
                    Evasion,
                    weapon == null
                        ? AttackDeliveryType.Natural
                        : AttackDeliveryType.EquippedWeapon,
                    weapon,
                    weapon == null ? 0 : 1));
            }

            return result;
        }
    }
}
