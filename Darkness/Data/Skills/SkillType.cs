using System;
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
        public List<EffectApplication> Effects { get; private set; }
        public SkillFunction Function { get; private set; }

        public SkillType(
            string id,
            string name,
            string description,
            SkillCostType costType,
            int costAmount,
            bool isPassive,
            List<EffectApplication> effects,
            SkillFunction function)
        {
            Id = id;
            Name = name;
            Description = description;
            CostType = costType;
            CostAmount = costAmount;
            IsPassive = isPassive;
            Effects = effects;
            Function = function;
        }
    }
}
