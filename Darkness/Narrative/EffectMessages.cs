namespace Darkness
{
    public static class EffectMessages
    {
        public static string StackApplied(
            string target,
            string effect,
            int stackCount)
        {
            return target + "에게 [" + effect + " " + stackCount +
                   "]이 부여되었다.";
        }

        public static string ForcedWait(
            string target,
            string effect,
            int remainingStacks)
        {
            if (remainingStacks <= 0)
            {
                return target + "은(는) [" + effect +
                       "] 때문에 움직이지 못했다. [" + effect +
                       "]이 해제되었다.";
            }

            return target + "은(는) [" + effect +
                   "] 때문에 움직이지 못했다. [" + effect + " " +
                   remainingStacks + "]";
        }

        public static string Triggered(string target, string effect)
        {
            return target + "의 [" + effect + "]이(가) 발동했다.";
        }

        public static string Consumed(string target, string effect)
        {
            return target + "의 [" + effect +
                   "]이(가) 발동한 뒤 사라졌다.";
        }
    }
}
