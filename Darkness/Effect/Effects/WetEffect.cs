using System;

namespace Darkness
{
    public class WetEffect : ActiveEffect
    {
        private const int EvasionPenalty = 10;
        private const int FrozenDamageBonus = 1;

        public WetEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyIncomingAttack(
            AttackContext context)
        {
            context.Evasion = Math.Max(
                0,
                context.Evasion - EvasionPenalty);
            return false;
        }

        public override bool ModifyIncomingDamage(
            DamageContext context)
        {
            if (!HasFrozenEffect(context.Target))
            {
                return false;
            }

            context.FinalDamage += FrozenDamageBonus;
            return false;
        }

        private static bool HasFrozenEffect(IDamageable target)
        {
            return target != null &&
                   target.Effects != null &&
                   target.Effects.Exists(effect =>
                       effect.Type != null &&
                       effect.Type.Id == "frozen");
        }
    }
}
