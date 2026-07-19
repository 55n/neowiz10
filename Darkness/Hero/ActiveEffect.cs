using System;

namespace Darkness
{
    public class ActiveEffect
    {
        public EffectType Type { get; private set; }
        public int StackCount { get; private set; }

        public ActiveEffect(EffectType type)
        {
            Type = type;
            StackCount = 1;
        }

        public void AddStack()
        {
            StackCount = Math.Min(Type.MaxStackCount, StackCount + 1);
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

    }
}
