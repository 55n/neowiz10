using System;
using System.Collections.Generic;

namespace Darkness
{
    public static class AttackResolver
    {
        private static readonly Random Random = new Random();

        public static AttackResult Resolve(AttackContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            List<ActiveEffect> consumedSourceEffects =
                ApplyOutgoingEffects(context);
            List<ActiveEffect> consumedTargetEffects =
                ApplyIncomingEffects(context);

            RemoveEffects(context.Source, consumedSourceEffects);
            RemoveEffects(context.Target, consumedTargetEffects);

            int hitChance = Math.Max(
                0,
                Math.Min(100, context.Accuracy - context.Evasion));
            if (Random.Next(100) >= hitChance)
            {
                return new AttackResult(false, 0);
            }

            DamageContext damageContext = new DamageContext(
                context.Source,
                context.Target,
                context.BaseDamage);
            DamageResolver.Resolve(damageContext);

            return new AttackResult(true, damageContext.FinalDamage);
        }

        private static List<ActiveEffect> ApplyOutgoingEffects(
            AttackContext context)
        {
            List<ActiveEffect> consumedEffects = new List<ActiveEffect>();
            if (context.Source.Effects == null)
            {
                return consumedEffects;
            }

            foreach (ActiveEffect effect in context.Source.Effects)
            {
                if (effect.ModifyOutgoingAttack(context))
                {
                    consumedEffects.Add(effect);
                }
            }

            return consumedEffects;
        }

        private static List<ActiveEffect> ApplyIncomingEffects(
            AttackContext context)
        {
            List<ActiveEffect> consumedEffects = new List<ActiveEffect>();
            if (context.Target.Effects == null)
            {
                return consumedEffects;
            }

            foreach (ActiveEffect effect in context.Target.Effects)
            {
                if (effect.ModifyIncomingAttack(context))
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
            if (target.Effects == null)
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
