using System.Collections.Generic;

namespace Darkness
{
    public class EchoMimicBehavior :
        IMonsterBehavior,
        ISkillOutcomeReflector
    {
        private int consecutiveWaits;
        private int healthBeforeSkill;
        private Dictionary<string, int> effectStacksBeforeSkill;
        private int reflectedDamage;
        private List<ReflectedEffect> reflectedEffects;
        public bool HasPendingSkillOutcome
        {
            get
            {
                return reflectedDamage > 0 ||
                       reflectedEffects.Count > 0;
            }
        }

        public EchoMimicBehavior()
        {
            effectStacksBeforeSkill = new Dictionary<string, int>();
            reflectedEffects = new List<ReflectedEffect>();
        }

        public void BeginSkillResolution(Monster monster)
        {
            healthBeforeSkill = monster.CurrentHealth;
            effectStacksBeforeSkill.Clear();
            foreach (ActiveEffect effect in monster.Effects)
            {
                effectStacksBeforeSkill[effect.Type.Id] =
                    effect.StackCount;
            }

            reflectedDamage = 0;
            reflectedEffects.Clear();
        }

        public void CompleteSkillResolution(Monster monster)
        {
            reflectedDamage = System.Math.Max(
                0,
                healthBeforeSkill - monster.CurrentHealth);

            foreach (ActiveEffect effect in monster.Effects)
            {
                int previousStacks;
                effectStacksBeforeSkill.TryGetValue(
                    effect.Type.Id,
                    out previousStacks);
                int addedStacks = effect.StackCount - previousStacks;
                if (addedStacks > 0)
                {
                    reflectedEffects.Add(new ReflectedEffect(
                        effect.Type.Id,
                        addedStacks));
                }
            }
        }

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            if (action == null)
            {
                return MonsterDecision.None(monster.State);
            }

            if (action.Action == PlayerActionType.Wait)
            {
                consecutiveWaits++;
                if (consecutiveWaits >= 3)
                {
                    return new MonsterDecision(
                        monster.State,
                        MonsterActionPlan.Vanish(),
                        "당신이 움직이지 않자 어둠 속의 기척 하나가 희미해진다.");
                }

                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "어둠 속의 누군가도 숨을 죽인 채 움직이지 않는다.");
            }

            consecutiveWaits = 0;

            if (action.Action == PlayerActionType.UseSkill)
            {
                if (HasPendingSkillOutcome)
                {
                    return new MonsterDecision(
                        MonsterState.Combat,
                        MonsterActionPlan.Reflect(
                            reflectedDamage,
                            reflectedEffects),
                        "당신이 일으킨 충격과 이변이 어둠 속에서 그대로 되돌아온다.");
                }

                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "어둠 속에서 당신과 똑같은 몸짓 소리만 되풀이된다.");
            }

            bool isTarget = action.TargetSlot == perception.CurrentSlot;
            if (action.Action == PlayerActionType.Attack && isTarget)
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.Attack(MonsterState.Combat),
                    "당신이 무기를 휘두르자 반대편에서도 같은 궤적의 바람이 갈라진다.");
            }

            if (action.Action == PlayerActionType.Defend)
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Defend(),
                    "어둠 속의 누군가도 동시에 자세를 낮춘다.");
            }

            if (action.Action == PlayerActionType.Talk && isTarget)
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "당신의 목소리가 한 박자 늦게 똑같은 억양으로 되돌아온다.");
            }

            if (action.Action == PlayerActionType.Search && isTarget)
            {
                return new MonsterDecision(
                    monster.State,
                    MonsterActionPlan.Wait(),
                    "당신이 더듬는 소리를 따라 반대편에서도 손끝이 벽을 훑는다.");
            }

            if (action.Action == PlayerActionType.ThrowItem && isTarget)
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.Attack(MonsterState.Combat),
                    "물건이 부딪힌 직후 같은 세기의 충격이 어둠 속에서 되돌아온다.");
            }

            return new MonsterDecision(
                monster.State,
                MonsterActionPlan.Wait(),
                "당신의 움직임을 닮은 소리가 어둠 속에서 뒤따른다.");
        }
    }
}
