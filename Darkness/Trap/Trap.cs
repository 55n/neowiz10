using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Trap : IDamageable, ISlotContent
    {
        public TrapType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int Evasion { get { return 0; } }
        public List<ActiveEffect> Effects { get; private set; }
        public bool IsActive { get; private set; }
        public string Id { get { return Type.Id; } }
        public string Name { get { return Type.Name; } }
        public RoomObjectType ObjectType { get { return RoomObjectType.Trap; } }

        public Trap(
            TrapType type)
        {
            Type = type;
            CurrentHealth = type.MaxHealth;
            Effects = new List<ActiveEffect>();
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void ReceiveDamage(int damage)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - Math.Max(0, damage));
            if (CurrentHealth == 0)
            {
                Deactivate();
            }
        }

        public virtual SlotInteractionResult React(
            PlayerActionContext context)
        {
            SlotInteractionResult result = new SlotInteractionResult();

            if (context.Action == PlayerActionType.Attack)
            {
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    this,
                    context.Actor.Type.Attack,
                    context.Actor.Type.Accuracy,
                    0));
            }

            if (!IsActive ||
                (context.Action != PlayerActionType.Search &&
                 context.Action != PlayerActionType.Attack))
            {
                return result;
            }

            result.RevealSlot = true;
            result.Messages.Add(Type.Description);
            result.Attacks.Add(new AttackContext(
                this,
                context.Actor,
                Type.Damage,
                Type.Accuracy,
                context.Actor.Type.Evasion));

            if (Type.IsSingleUse)
            {
                Deactivate();
            }

            return result;
        }
    }
}
