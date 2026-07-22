namespace Darkness
{
    public enum DoorClingerPartRole
    {
        Tentacle,
        Leg,
        Antenna,
        Claw
    }

    public class DoorClingerPartBehavior :
        IMonsterBehavior,
        IDefeatBehavior,
        ISearchHintBehavior
    {
        private readonly DoorClingerPartRole role;
        private int phase;

        public DoorClingerPartBehavior(
            DoorClingerPartRole role)
        {
            this.role = role;
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            int currentPhase = phase;
            phase = (phase + 1) % 4;

            switch (role)
            {
                case DoorClingerPartRole.Tentacle:
                    return DecideTentacle(currentPhase);
                case DoorClingerPartRole.Leg:
                    return DecideLeg(currentPhase);
                case DoorClingerPartRole.Antenna:
                    return DecideAntenna(currentPhase);
                case DoorClingerPartRole.Claw:
                    return DecideClaw(currentPhase);
                default:
                    return MonsterDecision.None(
                        MonsterState.Combat);
            }
        }

        public string GetSearchHint(
            Monster monster,
            PlayerActionContext context)
        {
            return "손끝으로 팽팽하게 차오른 마력이 느껴진다.";
        }

        public DefeatBehaviorResult ResolveDefeat(Monster monster)
        {
            return DefeatBehaviorResult.Complete(
                "문붙이의 한 부위가 말라붙자 문틀을 채운 살덩이 전체가 한층 작게 수축했다.");
        }

        private static MonsterDecision DecideTentacle(int phase)
        {
            if (phase == 0)
            {
                return Attack(
                    NarrativeTokens.Actor +
                    "의 촉수가 발목을 휘감으려 든다.");
            }

            if (phase == 2)
            {
                return Attack(
                    NarrativeTokens.Actor +
                    "의 촉수가 허리를 향해 거칠게 휘어진다.");
            }

            return Wait(
                NarrativeTokens.Actor +
                "의 촉수가 문틀로 움츠러들며 다음 움직임을 노린다.");
        }

        private static MonsterDecision DecideLeg(int phase)
        {
            if (phase == 0 || phase == 2)
            {
                return Defend(
                    NarrativeTokens.Actor +
                    "이(가) 벽과 바닥을 단단히 짚어 몸을 지탱한다.");
            }

            return Attack(
                NarrativeTokens.Actor +
                "이(가) 바닥을 박차며 정강이를 후려친다.");
        }

        private static MonsterDecision DecideAntenna(int phase)
        {
            if (phase == 1)
            {
                return DrainFocus(
                    NarrativeTokens.Actor +
                    "가 관자놀이를 스치며 정신을 흐트러뜨린다.");
            }

            return Wait(
                NarrativeTokens.Actor +
                "가 공기 중의 마력과 호흡을 따라 가늘게 떨린다.");
        }

        private static MonsterDecision DecideClaw(int phase)
        {
            if (phase == 0)
            {
                return Wait(
                    NarrativeTokens.Actor +
                    "가 높이 들리며 끝부분을 당신에게 겨눈다.");
            }

            if (phase == 1)
            {
                return Attack(
                    NarrativeTokens.Actor +
                    "가 크게 휘둘러지며 살점을 노린다.");
            }

            if (phase == 2)
            {
                return Wait(
                    NarrativeTokens.Actor +
                    "가 바닥에 박혔다가 천천히 빠져나온다.");
            }

            return Wait(
                NarrativeTokens.Actor +
                "가 몸통 옆으로 접혀 다시 힘을 모은다.");
        }

        private static MonsterDecision Attack(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(
                    MonsterState.Combat),
                message);
        }

        private static MonsterDecision Wait(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Wait(),
                message);
        }

        private static MonsterDecision Defend(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Defend(),
                message);
        }

        private static MonsterDecision DrainFocus(string message)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.DrainFocus(),
                message);
        }
    }
}
