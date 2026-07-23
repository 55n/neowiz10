using System;
using System.Collections.Generic;

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
                context.Skill.Name));
            result.Attacks.AddRange(effectResult.Attacks);
            result.Damages.AddRange(effectResult.Damages);
            result.DurabilityRequests.AddRange(
                effectResult.DurabilityRequests);

            Monster movingMonster = context.User as Monster;
            if (context.Skill.FollowUpType == SkillFollowUpType.Move &&
                movingMonster != null && context.FollowUpSlot != null)
            {
                result.MonsterMoves.Add(new MonsterMoveRequest(
                    movingMonster,
                    context.FollowUpSlot));
            }

            foreach (IEffectTarget target in
                     effectResult.AffectedEffectTargets)
            {
                if (!string.IsNullOrEmpty(
                        context.Skill.ResultMessage))
                {
                    string message = CreateResultMessage(
                        context,
                        target);
                    if (!string.IsNullOrEmpty(message))
                    {
                        result.Messages.Add(message);
                    }
                }
            }

            if (effectResult.AffectedEffectTargets.Count == 0 &&
                !string.IsNullOrEmpty(context.Skill.ResultMessage) &&
                context.SelectedTargets.Count > 0)
            {
                IEffectTarget target =
                    context.SelectedTargets[0] as IEffectTarget;
                string message = CreateResultMessage(
                    context,
                    target);
                if (!string.IsNullOrEmpty(message))
                {
                    result.Messages.Add(message);
                }
            }

            AddAppliedEffectMessages(
                context,
                effectResult,
                result);

            return result;
        }

        private static string CreateResultMessage(
            SkillUseContext context,
            IEffectTarget target)
        {
            if (context == null || context.Skill == null ||
                target == null || string.IsNullOrEmpty(
                    context.Skill.ResultMessage))
            {
                return null;
            }

            string targetToken = ReferenceEquals(
                target,
                context.User)
                ? NarrativeTokens.Actor
                : NarrativeTokens.Target;
            return context.Skill.ResultMessage.Replace(
                "{0}",
                targetToken);
        }

        private static void AddAppliedEffectMessages(
            SkillUseContext context,
            EffectResolveResult effectResult,
            SkillUseResult result)
        {
            List<AppliedEffectResult> latestResults =
                new List<AppliedEffectResult>();
            foreach (AppliedEffectResult applied in
                     effectResult.AppliedEffects)
            {
                if (applied == null || applied.Target == null ||
                    applied.Effect == null)
                {
                    continue;
                }

                int existingIndex = latestResults.FindIndex(
                    existing =>
                        ReferenceEquals(
                            existing.Target,
                            applied.Target) &&
                        existing.Effect.Id == applied.Effect.Id);
                if (existingIndex >= 0)
                {
                    latestResults[existingIndex] = applied;
                }
                else
                {
                    latestResults.Add(applied);
                }
            }

            foreach (AppliedEffectResult applied in latestResults)
            {
                string targetToken;
                if (ReferenceEquals(
                        applied.Target,
                        context.User))
                {
                    targetToken = NarrativeTokens.Actor;
                }
                else if (context.SelectedTargets.Exists(
                    target => ReferenceEquals(
                        target,
                        applied.Target)))
                {
                    targetToken = NarrativeTokens.Target;
                }
                else
                {
                    targetToken = applied.Target.Name;
                }
                result.Messages.Add(
                    EffectMessages.StackApplied(
                        targetToken,
                        applied.Effect.Name,
                        applied.StackCount));
            }
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
