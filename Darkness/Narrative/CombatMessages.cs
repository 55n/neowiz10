namespace Darkness
{
    public static class CombatMessages
    {
        public static string AttackStarted(string attacker, string target)
        {
            return attacker + " 이(가) " + target + " 을(를) 공격했다";
        }

        public static string DefenseStanceTaken()
        {
            return "당신은 방어 자세를 취했다.";
        }

        public static string DefenseFocusRestored(int amount)
        {
            return "당신은 적의 움직임에 집중해 집중력 " + amount +
                   "을(를) 회복했다.";
        }

        public static string ObjectDestroyed(string target)
        {
            return target + "이(가) 부서졌다.";
        }

        public static string DamageReceived(string target, int damage)
        {
            return target + " 이(가) " + damage + " 의 피해를 입었다";
        }

        public static string DamageEvaded(string target)
        {
            return target + " 이(가) 피해를 회피했다";
        }

        public static string FocusDrained(
            string target,
            int amount)
        {
            return target + " 이(가) 집중력 " + amount +
                   "을(를) 잃었다";
        }

        public static string DamageTaken(string enemy, string damage)
        {
            return "[" + enemy + "]에게 " + damage + "의 데미지를 받았습니다";
        }

        public static string MonsterAction()
        {
            return "??? 은(는) 크르륵 거린다";
        }

        public static string MonsterAttackStarted()
        {
            return "어둠 속의 정체가 드러났다. 고블린이 달려든다";
        }

        public static string PlayerDodged()
        {
            return "당신은 가까스로 공격을 피했다";
        }

        public static string PlayerHit(string monster, int damage)
        {
            return monster + "의 공격이 스쳤다. 생명력 " + damage + "을(를) 잃었다";
        }

        public static string PlayerDied()
        {
            return "당신은 어둠 속에서 쓰러졌다";
        }

        public static string GameOver()
        {
            return "[게임 오버]";
        }

        public static string NormalAttackResult(string monster, int damage)
        {
            return "당신은 " + monster + " 을(를) 공격해 " + damage + "의 데미지를 주었다";
        }

        public static string SkillAttackResult(string skill, string monster, int damage)
        {
            return "당신은 " + skill + " 을(를) 사용해 " + monster + " 에게 " + damage + "의 데미지를 주었다";
        }

        public static string MonsterDefeated(string monster)
        {
            return monster + " 이(가) 쓰러졌다";
        }

        public static string MonsterLootDropped()
        {
            return "무언가가 바닥에 떨어졌다";
        }
    }
}
