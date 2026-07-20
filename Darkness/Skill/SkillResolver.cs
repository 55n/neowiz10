using System;

namespace Darkness
{
    public class SkillResolver
    {
        private readonly EffectResolver effectResolver;

        public SkillResolver()
        {
            effectResolver = new EffectResolver();
        }

        public SkillUseResult Resolve(SkillUseContext context)
        {
            SkillUseResult result = new SkillUseResult();
            if (!CanUse(context))
            {
                AddFailureMessage(context, result);
                return result;
            }

            EffectPlan plan;
            if (!effectResolver.TryPrepare(
                    context.Skill.Effects,
                    EffectContext.FromSkill(context),
                    out plan) ||
                !SkillCostResolver.TryPay(
                    context.User,
                    context.Skill))
            {
                AddFailureMessage(context, result);
                return result;
            }

            EffectResolveResult effectResult =
                effectResolver.Execute(plan);

            result.Succeeded = true;
            result.Messages.Add(SkillMessages.Used(
                context.User.Name,
                context.Skill.Name));
            result.Attacks.AddRange(effectResult.Attacks);
            result.Damages.AddRange(effectResult.Damages);
            result.DurabilityRequests.AddRange(
                effectResult.DurabilityRequests);

            foreach (IEffectTarget target in
                     effectResult.AffectedEffectTargets)
            {
                string message = SkillMessages.Result(
                    context.Skill,
                    target);
                if (!string.IsNullOrEmpty(message))
                {
                    result.Messages.Add(message);
                }
            }

            return result;
        }

        private static bool CanUse(SkillUseContext context)
        {
            if (context == null || context.Skill == null ||
                context.Skill.IsPassive ||
                !context.User.KnowsSkill(context.Skill.Id) ||
                !SkillCostResolver.CanPay(
                    context.User,
                    context.Skill))
            {
                return false;
            }

            if (context.Skill.TargetingType ==
                SkillTargetingType.None)
            {
                return context.SelectedTargets.Count == 0 &&
                       HasRequiredEquipment(context);
            }

            return context.SelectedTargets.Count > 0 &&
                   HasRequiredEquipment(context);
        }

        private static bool HasRequiredEquipment(
            SkillUseContext context)
        {
            if (context.Skill.AttackType != SkillAttackType.Weapon)
            {
                return true;
            }

            IEquipmentUser user = context.User as IEquipmentUser;
            return user != null &&
                   user.GetEquippedItem(EquipmentSlot.Weapon) != null;
        }

        private static void AddFailureMessage(
            SkillUseContext context,
            SkillUseResult result)
        {
            if (context != null && context.Skill != null)
            {
                result.Messages.Add(
                    SkillMessages.CannotUse(context.Skill.Name));
            }
        }
    }
}
