namespace Darkness
{
    public class SpiritBarrierEffect : ActiveEffect
    {
        private const int DamageReduction = 5;

        public override int IncomingDamagePriority
        {
            get { return 50; }
        }

        public SpiritBarrierEffect(
            EffectType type,
            object source = null)
            : base(type, source)
        {
        }

        public override bool ModifyIncomingDamage(
            DamageContext context)
        {
            if (context.FinalDamage <= 0)
            {
                return false;
            }

            context.FinalDamage = System.Math.Max(
                0,
                context.FinalDamage - DamageReduction);
            return true;
        }
    }
}
