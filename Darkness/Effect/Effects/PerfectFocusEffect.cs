namespace Darkness
{
    public class PerfectFocusEffect : ActiveEffect
    {
        private const int AccuracyBonus = 100;

        public PerfectFocusEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyOutgoingAttack(AttackContext context)
        {
            context.Accuracy += AccuracyBonus;
            return true;
        }
    }
}
