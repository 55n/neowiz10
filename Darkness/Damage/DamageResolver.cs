using System;
using System.Collections.Generic;

namespace Darkness
{
    public static class DamageResolver
    {
        public static void Resolve(DamageContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IDamageable source = context.Source as IDamageable;
            List<ActiveEffect> consumedSourceEffects =
                ApplyOutgoingEffects(source, context);
            List<ActiveEffect> consumedTargetEffects =
                ApplyIncomingEffects(context.Target, context);

            context.FinalDamage = Math.Max(0, context.FinalDamage);
            context.Target.ReceiveDamage(context.FinalDamage);

            RemoveEffects(source, consumedSourceEffects);
            RemoveEffects(context.Target, consumedTargetEffects);
        }

        private static List<ActiveEffect> ApplyOutgoingEffects(
            IDamageable source,
            DamageContext context)
        {
            List<ActiveEffect> consumedEffects = new List<ActiveEffect>();
            if (source == null || source.Effects == null)
            {
                return consumedEffects;
            }

            foreach (ActiveEffect effect in source.Effects)
            {
                if (effect.ModifyOutgoingDamage(context))
                {
                    consumedEffects.Add(effect);
                }
            }

            return consumedEffects;
        }

        private static List<ActiveEffect> ApplyIncomingEffects(
            IDamageable target,
            DamageContext context)
        {
            List<ActiveEffect> consumedEffects = new List<ActiveEffect>();
            if (target.Effects == null)
            {
                return consumedEffects;
            }

            foreach (ActiveEffect effect in target.Effects)
            {
                if (effect.ModifyIncomingDamage(context))
                {
                    consumedEffects.Add(effect);
                }
            }

            return consumedEffects;
        }

        private static void RemoveEffects(
            IDamageable target,
            List<ActiveEffect> effects)
        {
            if (target == null || target.Effects == null)
            {
                return;
            }

            foreach (ActiveEffect effect in effects)
            {
                target.Effects.Remove(effect);
            }
        }
    }
}
