using System;
using System.Collections.Generic;

namespace Darkness
{
    public class EffectResolver
    {
        private static readonly Random Random = new Random();
        private readonly ActiveEffectFactory activeEffectFactory;
        private readonly EffectTargetResolver targetResolver;

        public EffectResolver()
        {
            activeEffectFactory = new ActiveEffectFactory();
            targetResolver = new EffectTargetResolver();
        }

        public bool TryPrepare(
            IEnumerable<EffectApplication> applications,
            EffectContext context,
            out EffectPlan plan)
        {
            plan = new EffectPlan(context == null ? null : context.Source);
            if (applications == null || context == null)
            {
                return false;
            }

            foreach (EffectApplication application in applications)
            {
                if (application == null ||
                    !TryPrepareApplication(application, context, plan))
                {
                    return false;
                }
            }

            return plan.Effects.Count > 0;
        }

        public EffectResolveResult Execute(EffectPlan plan)
        {
            EffectResolveResult result = new EffectResolveResult();
            if (plan == null)
            {
                return result;
            }

            foreach (PlannedEffect effect in plan.Effects)
            {
                int chance = Math.Max(
                    0,
                    Math.Min(100, effect.Application.ApplyChance));
                if (Random.Next(100) >= chance)
                {
                    continue;
                }

                if (effect.Application.Operation ==
                    EffectOperation.ApplyStatus)
                {
                    ApplyStatus(effect, result);
                }
                else if (effect.Application.Operation ==
                         EffectOperation.Damage)
                {
                    IDamageable target = effect.Target as IDamageable;
                    result.Damages.Add(new DamageContext(
                        plan.Source,
                        target,
                        effect.Application.Magnitude));
                }
                else if (effect.Application.Operation ==
                         EffectOperation.Attack)
                {
                    ISkillUser source = (ISkillUser)plan.Source;
                    IDamageable target = (IDamageable)effect.Target;
                    int damage = effect.Application.UsesFixedAttackDamage
                        ? Math.Max(0, effect.Application.Magnitude)
                        : Math.Max(
                            0,
                            source.Attack *
                            effect.Application.Magnitude / 100);
                    Item usedItem = null;
                    if (effect.Application.AttackDeliveryType ==
                        AttackDeliveryType.EquippedWeapon)
                    {
                        IEquipmentUser equipmentUser =
                            source as IEquipmentUser;
                        usedItem = equipmentUser == null
                            ? null
                            : equipmentUser.GetEquippedItem(
                                EquipmentSlot.Weapon);
                    }
                    result.Attacks.Add(new AttackContext(
                        source as IDamageable,
                        target,
                        damage,
                        source.Accuracy +
                        effect.Application.AccuracyModifier,
                        target.Evasion,
                        effect.Application.AttackDeliveryType,
                        usedItem,
                        usedItem == null ? 0 : 1));
                }
                else if (effect.Application.Operation ==
                         EffectOperation.SetEquipmentDurability)
                {
                    result.DurabilityRequests.Add(
                        new EquipmentDurabilityRequest(
                            (IEquipmentUser)effect.Target,
                            effect.Application.EquipmentSlot,
                            effect.Application.Magnitude));
                }
            }

            return result;
        }

        private bool TryPrepareApplication(
            EffectApplication application,
            EffectContext context,
            EffectPlan plan)
        {
            List<object> targets = targetResolver.Resolve(
                application.Target,
                context);
            if (targets.Count == 0)
            {
                return false;
            }

            foreach (object target in targets)
            {
                if (application.Operation == EffectOperation.ApplyStatus)
                {
                    IEffectTarget receiver = target as IEffectTarget;
                    ActiveEffect activeEffect = activeEffectFactory.Create(
                        application.EffectId);
                    if (receiver == null || activeEffect == null)
                    {
                        return false;
                    }

                    if (CanApply(receiver, activeEffect))
                    {
                        plan.Effects.Add(new PlannedEffect(
                            application,
                            receiver,
                            activeEffect));
                    }
                }
                else if (application.Operation == EffectOperation.Damage)
                {
                    IDamageable damageable = target as IDamageable;
                    if (damageable == null)
                    {
                        return false;
                    }

                    plan.Effects.Add(new PlannedEffect(
                        application,
                        damageable,
                        null));
                }
                else if (application.Operation == EffectOperation.Attack)
                {
                    ISkillUser source = context.Source as ISkillUser;
                    IDamageable damageable = target as IDamageable;
                    if (source == null ||
                        !(context.Source is IDamageable) ||
                        damageable == null)
                    {
                        return false;
                    }

                    plan.Effects.Add(new PlannedEffect(
                        application,
                        damageable,
                        null));
                }
                else if (application.Operation ==
                         EffectOperation.SetEquipmentDurability)
                {
                    IEquipmentUser equipmentUser =
                        target as IEquipmentUser;
                    Item item = equipmentUser == null
                        ? null
                        : equipmentUser.GetEquippedItem(
                            application.EquipmentSlot);
                    if (item == null || !item.UsesDurability)
                    {
                        return false;
                    }

                    plan.Effects.Add(new PlannedEffect(
                        application,
                        equipmentUser,
                        null));
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CanApply(
            IEffectTarget target,
            ActiveEffect effect)
        {
            ActiveEffect existing = target.Effects.Find(
                active => active.Type.Id == effect.Type.Id);
            return existing == null ||
                   effect.Type.IsStackable &&
                   existing.StackCount < effect.Type.MaxStackCount;
        }

        private static void ApplyStatus(
            PlannedEffect effect,
            EffectResolveResult result)
        {
            IEffectTarget target = (IEffectTarget)effect.Target;
            int stackCount = Math.Max(1, effect.Application.StackCount);
            for (int i = 0; i < stackCount; i++)
            {
                target.ApplyEffect(effect.ActiveEffect);
            }

            result.AffectedEffectTargets.Add(target);
        }
    }
}
