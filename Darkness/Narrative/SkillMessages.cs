namespace Darkness
{
    public static class SkillMessages
    {
        public static string Activated(string skill)
        {
            return "[" + skill + "] 스킬이 발동됩니다.";
        }

        public static string Used(string caster, string skill)
        {
            return caster + " 은(는) [" + skill + "] 을(를) 사용했다";
        }

        public static string Result(SkillType skill, IEffectTarget target)
        {
            if (skill == null || target == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(skill.ResultMessage))
            {
                return string.Format(skill.ResultMessage, target.Name);
            }

            return target.Name + "에게 [" + skill.Name +
                   "]의 효과가 적용되었다.";
        }

        public static string CannotUse(string skill)
        {
            return "[" + skill + "] 스킬을 사용할 수 없다";
        }

        public static string NotImplemented(string skill)
        {
            return "[" + skill + "] 스킬 효과는 아직 구현되지 않았다";
        }
    }
}
