namespace Darkness
{
    public class FixedEffect : ActiveEffect
    {
        private const int FixedEvasion = -100;

        public FixedEffect(
            EffectType type,
            object source = null)
            : base(type, source)
        {
        }

        public override bool ModifyIncomingAttack(
            AttackContext context)
        {
            context.Evasion = FixedEvasion;
            return false;
        }
    }
}
