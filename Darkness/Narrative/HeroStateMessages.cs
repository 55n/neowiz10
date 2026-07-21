namespace Darkness
{
    public static class HeroStateMessages
    {
        public static string Restored(
            int restoredHealth,
            int restoredFocus)
        {
            if (restoredHealth > 0 && restoredFocus > 0)
            {
                return "당신은 휴식을 취해 생명력 " + restoredHealth +
                       "과 집중력 " + restoredFocus + "을(를) 회복했다.";
            }

            if (restoredHealth > 0)
            {
                return "당신은 휴식을 취해 생명력 " + restoredHealth +
                       "을(를) 회복했다.";
            }

            if (restoredFocus > 0)
            {
                return "당신은 휴식을 취해 집중력 " + restoredFocus +
                       "을(를) 회복했다.";
            }

            return null;
        }
    }
}
