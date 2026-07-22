using System;
using System.Collections.Generic;

namespace Darkness
{
    public class ActiveEffect
    {
        public EffectType Type { get; private set; }
        public object Source { get; private set; }
        public int StackCount { get; private set; }
        public virtual int IncomingDamagePriority
        {
            get { return 0; }
        }
        public virtual bool ForcesWait
        {
            get { return false; }
        }

        public ActiveEffect(EffectType type, object source = null)
        {
            Type = type;
            Source = source;
            StackCount = 1;
        }

        public void AddStack()
        {
            StackCount = Math.Min(Type.MaxStackCount, StackCount + 1);
        }

        public bool ConsumeStack()
        {
            StackCount = Math.Max(0, StackCount - 1);
            return StackCount == 0;
        }

        public virtual int GetAttackBonus()
        {
            return 0;
        }

        public virtual int GetDefenseBonus()
        {
            return 0;
        }

        public virtual int ModifyWeaponAttackBonus(
            int weaponAttackBonus)
        {
            return weaponAttackBonus;
        }

        public virtual bool ModifyOutgoingDamage(DamageContext context)
        {
            return false;
        }

        public virtual bool ModifyOutgoingAttack(AttackContext context)
        {
            return false;
        }

        public virtual bool ModifyIncomingAttack(AttackContext context)
        {
            return false;
        }

        public virtual bool ModifyIncomingDamage(DamageContext context)
        {
            return false;
        }

        public virtual bool CanApplyTo(IEffectTarget target)
        {
            return true;
        }

        public virtual IEnumerable<string> GetTriggeredEffectIds(
            IEffectTarget target)
        {
            return new string[0];
        }

        public virtual void OnStackIncreased(
            EffectStackIncreaseContext context)
        {
        }

        public virtual void OnTurnEnd(EffectTurnContext context)
        {
        }

        public virtual bool IsActiveInRoom(Room room)
        {
            return true;
        }
    }
}
