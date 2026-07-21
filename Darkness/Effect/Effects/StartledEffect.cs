using System;

namespace Darkness
{
    public class StartledEffect : ActiveEffect
    {
        private const int AccuracyPenalty = 10;
        private const int EvasionPenalty = 10;

        public StartledEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyOutgoingAttack(
            AttackContext context)
        {
            context.Accuracy = Math.Max(
                0,
                context.Accuracy - AccuracyPenalty);
            return false;
        }

        public override bool ModifyIncomingAttack(
            AttackContext context)
        {
            context.Evasion = Math.Max(
                0,
                context.Evasion - EvasionPenalty);
            return false;
        }
    }
}
