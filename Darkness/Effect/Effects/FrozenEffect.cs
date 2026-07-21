namespace Darkness
{
    public class FrozenEffect : ActiveEffect
    {
        private const int IncomingDamageBonus = 1;

        public FrozenEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyIncomingDamage(
            DamageContext context)
        {
            context.FinalDamage += IncomingDamageBonus;
            return false;
        }
    }
}
