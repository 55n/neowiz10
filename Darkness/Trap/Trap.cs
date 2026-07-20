using System;
using System.Collections.Generic;

namespace Darkness
{
    public class Trap : IDamageable, ISlotContent
    {
        public TrapType Type { get; private set; }
        public int CurrentHealth { get; private set; }
        public int Defense
        {
            get
            {
                int bonus = 0;
                foreach (ActiveEffect effect in Effects)
                {
                    bonus += effect.GetDefenseBonus();
                }

                return Math.Max(0, Type.Defense + bonus);
            }
        }
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
                Item weapon = context.Actor.GetEquippedItem(
                    EquipmentSlot.Weapon);
                result.Attacks.Add(new AttackContext(
                    context.Actor,
                    this,
                    context.Actor.Attack,
                    context.Actor.Accuracy,
                    0,
                    weapon == null
                        ? AttackDeliveryType.Natural
                        : AttackDeliveryType.EquippedWeapon,
                    weapon,
                    weapon == null ? 0 : 1));
            }

            if (!IsActive ||
                (context.Action != PlayerActionType.Search &&
                 context.Action != PlayerActionType.Attack))
            {
                return result;
            }

            result.Messages.Add(Type.Description);
            result.Attacks.Add(new AttackContext(
                this,
                context.Actor,
                Type.Damage,
                Type.Accuracy,
                context.Actor.Type.Evasion,
                AttackDeliveryType.Trap,
                null,
                0));

            if (Type.IsSingleUse)
            {
                Deactivate();
            }

            return result;
        }
    }
}
