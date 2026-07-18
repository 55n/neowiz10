using System;
using System.Collections.Generic;

namespace Darkness
{
    public class ArrowTrap : Trap
    {
        public const string TypeId = "arrow_trap";

        public ArrowTrap(
            TrapType type,
            int slotIndex)
            : base(type, slotIndex, TypeId)
        {
        }

        public override void Evaluate(
            EncounterActionContext context,
            IList<ReactionResult> results)
        {
            if (!IsActive || context == null || context.TargetSlotIndex != SlotIndex)
            {
                return;
            }

            bool triggered = context.Signals.Contains(ActionSignal.Contact) ||
                             context.Signals.Contains(ActionSignal.HostileAction) ||
                             context.Signals.Contains(ActionSignal.Impact);
            if (!triggered)
            {
                return;
            }

            results.Add(ReactionResult.DamageActor(
                this,
                Type.Damage,
                Type.TriggerEffects,
                "벽의 틈에서 화살이 발사된다."));
            if (Type.IsSingleUse)
            {
                Deactivate();
            }
        }
    }
}
