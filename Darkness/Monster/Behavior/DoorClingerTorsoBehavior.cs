using System;

namespace Darkness
{
    public class DoorClingerTorsoBehavior :
        IMonsterBehavior,
        IDefeatBehavior,
        ISearchHintBehavior
    {
        private readonly Func<ISlotContent> createMagicCore;
        private int phase;

        public DoorClingerTorsoBehavior(
            Func<ISlotContent> createMagicCore)
        {
            if (createMagicCore == null)
            {
                throw new ArgumentNullException(
                    "createMagicCore");
            }

            this.createMagicCore = createMagicCore;
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            int currentPhase = phase;
            phase = (phase + 1) % 4;

            if (currentPhase == 2)
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.Attack(
                        MonsterState.Combat),
                    NarrativeTokens.Actor +
                    " 전체가 문틀에서 튀어나와 충돌한다.");
            }

            if (currentPhase == 3)
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.Defend(),
                    NarrativeTokens.Actor +
                    "이(가) 수축하며 마핵을 두꺼운 살로 감싼다.");
            }

            string message = currentPhase == 0
                ? NarrativeTokens.Actor +
                  " 내부에서 마력이 둔하게 맥동한다."
                : NarrativeTokens.Actor +
                  "이(가) 다른 부위들을 잡아당기며 형태를 유지한다.";
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Wait(),
                message);
        }

        public DefeatBehaviorResult ResolveDefeat(
            Monster monster)
        {
            return DefeatBehaviorResult.ReplaceWith(
                createMagicCore(),
                "문붙이의 몸통이 무너지며 안쪽에 박혀 있던 마핵이 드러났다.");
        }

        public string GetSearchHint(
            Monster monster,
            PlayerActionContext context)
        {
            return "손끝으로 팽팽하게 차오른 마력이 느껴진다.";
        }
    }
}
