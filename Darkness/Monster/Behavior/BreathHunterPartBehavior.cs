namespace Darkness
{
    public class BreathHunterPartBehavior : IMonsterBehavior
    {
        private readonly BreathHunterBody body;
        private readonly BreathHunterPartRole role;

        public BreathHunterPartBehavior(
            BreathHunterBody body,
            BreathHunterPartRole role)
        {
            this.body = body;
            this.role = role;
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            if (!body.IsAbdomenAlive)
            {
                return Wait(monster, null);
            }

            int phase = body.CombatTurn % 6;
            if (role == BreathHunterPartRole.LeftForeleg && phase == 0)
            {
                return Attack(
                    "왼쪽 앞다리가 바닥을 훑으며 당신을 후려친다.");
            }

            if (role == BreathHunterPartRole.RightForeleg &&
                (phase == 1 || phase == 5))
            {
                return Attack(
                    "오른쪽 앞다리가 어둠을 가르며 내려찍힌다.");
            }

            if (role == BreathHunterPartRole.Head && phase == 2)
            {
                return Wait(
                    monster,
                    "거대한 송곳니가 맞부딪히는 소리가 바로 앞에서 들린다.");
            }

            if (role == BreathHunterPartRole.Head && phase == 3)
            {
                return Attack(
                    "예고했던 거대한 송곳니가 당신을 물어뜯는다.");
            }

            if (role == BreathHunterPartRole.Spinneret && phase == 4)
            {
                Hero hero = perception == null ||
                    perception.PlayerAction == null
                    ? null
                    : perception.PlayerAction.Actor;
                bool isAlreadyBound = hero != null &&
                    hero.Effects.Exists(effect =>
                        effect.Type.Id == "bind");
                if (!isAlreadyBound)
                {
                    return new MonsterDecision(
                        MonsterState.Combat,
                        MonsterActionPlan.UseSkill("web_bind"),
                        "방적기관에서 뿜어진 실이 다시 당신의 팔다리를 휘감는다.");
                }
            }

            return Wait(monster, null);
        }

        private static MonsterDecision Attack(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(MonsterState.Combat),
                message);
        }

        private static MonsterDecision Wait(
            Monster monster,
            string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return MonsterDecision.None(monster.State);
            }

            return new MonsterDecision(
                monster.State,
                MonsterActionPlan.Wait(),
                message);
        }
    }
}
