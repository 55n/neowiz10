namespace Darkness
{
    public class GuardianBlessingEffect : ActiveEffect
    {
        public GuardianBlessingEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyIncomingDamage(DamageContext context)
        {
            if (context.FinalDamage < context.Target.CurrentHealth)
            {
                return false;
            }

            context.FinalDamage =
                System.Math.Max(0, context.Target.CurrentHealth - 1);
            return true;
        }
    }
}
