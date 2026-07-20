using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public static class SkillScreen
    {
        public static SelectionNode BuildNode(
            Hero hero,
            SkillData skillData,
            SelectionNode parent)
        {
            SelectionNode node = new SelectionNode(
                "skill-selection",
                BuildResourceDescription(hero),
                new List<SelectionOption>(),
                parent);

            List<SkillType> learnedSkills = skillData.SkillTypes.Values
                .Where(skill => hero.KnowsSkill(skill.Id))
                .OrderBy(skill => skill.Name)
                .ToList();

            foreach (SkillType skill in learnedSkills)
            {
                bool canUse = !skill.IsPassive &&
                              SkillCostResolver.CanPay(hero, skill);
                node.Options.Add(new SelectionOption(
                    BuildSkillText(skill),
                    skill.Description,
                    canUse,
                    null,
                    new SkillSelection(skill.Id)));
            }

            if (learnedSkills.Count == 0)
            {
                node.Options.Add(new SelectionOption(
                    "[배운 스킬이 없다]",
                    "",
                    false,
                    null));
            }

            node.Options.Add(new SelectionOption(
                "돌아가기",
                "전투 선택지로 돌아간다",
                true,
                parent));

            return node;
        }

        private static string BuildSkillText(SkillType skill)
        {
            string costName = skill.CostType == SkillCostType.Focus
                ? "집중"
                : "마석";
            return "[" + costName + " " + skill.CostAmount + "] " +
                   skill.Name;
        }

        private static string BuildResourceDescription(Hero hero)
        {
            return "집중 " + hero.CurrentFocus + "/" + hero.Type.MaxFocus +
                   " | 마석 " + SkillCostResolver.CountMagicStones(hero);
        }
    }
}
