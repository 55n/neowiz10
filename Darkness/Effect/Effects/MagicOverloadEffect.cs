namespace Darkness
{
    public class MagicOverloadEffect : ActiveEffect
    {
        private const int ExplosionStackCount = 10;
        private const int ExplosionDamage = 9999;

        public MagicOverloadEffect(
            EffectType type,
            object source = null)
            : base(type, source)
        {
        }

        public override void OnStackIncreased(
            EffectStackIncreaseContext context)
        {
            if (context == null ||
                context.PreviousStackCount >= ExplosionStackCount ||
                context.CurrentStackCount < ExplosionStackCount)
            {
                return;
            }

            context.AddDamage(
                ExplosionDamage,
                true);
        }
    }
}
