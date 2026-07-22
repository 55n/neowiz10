using System.Collections.Generic;

namespace Darkness
{
    public class SkillType
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public SkillCostType CostType { get; private set; }
        public int CostAmount { get; private set; }
        public bool IsPassive { get; private set; }
        public SkillTargetingType TargetingType { get; private set; }
        public SkillAttackType AttackType { get; private set; }
        public List<EffectApplication> Effects { get; private set; }
        public string ResultMessage { get; private set; }
        public SkillFollowUpType FollowUpType { get; private set; }

        public SkillType(
            string id,
            string name,
            string description,
            SkillCostType costType,
            int costAmount,
            bool isPassive,
            List<EffectApplication> effects,
            string resultMessage = null)
            : this(
                id,
                name,
                description,
                costType,
                costAmount,
                isPassive,
                SkillTargetingType.None,
                SkillAttackType.None,
                effects,
                resultMessage)
        {
        }

        public SkillType(
            string id,
            string name,
            string description,
            SkillCostType costType,
            int costAmount,
            bool isPassive,
            SkillTargetingType targetingType,
            List<EffectApplication> effects,
            string resultMessage = null)
            : this(
                id,
                name,
                description,
                costType,
                costAmount,
                isPassive,
                targetingType,
                SkillAttackType.None,
                effects,
                resultMessage)
        {
        }

        public SkillType(
            string id,
            string name,
            string description,
            SkillCostType costType,
            int costAmount,
            bool isPassive,
            SkillTargetingType targetingType,
            SkillAttackType attackType,
            List<EffectApplication> effects,
            string resultMessage = null,
            SkillFollowUpType followUpType = SkillFollowUpType.None)
        {
            Id = id;
            Name = name;
            Description = description;
            CostType = costType;
            CostAmount = costAmount;
            IsPassive = isPassive;
            TargetingType = targetingType;
            AttackType = attackType;
            Effects = effects;
            ResultMessage = resultMessage;
            FollowUpType = followUpType;
        }
    }
}
