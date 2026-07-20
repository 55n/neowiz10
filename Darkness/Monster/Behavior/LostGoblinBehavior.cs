using System;

namespace Darkness
{
    public class LostGoblinBehavior : IMonsterBehavior
    {
        private const string RushSkillId = "goblin_rush";

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            if (monster == null || perception == null ||
                perception.PlayerAction == null)
            {
                return MonsterDecision.None(
                    monster == null
                        ? MonsterState.Indifferent
                        : monster.State);
            }

            switch (monster.State)
            {
                case MonsterState.Indifferent:
                    return DecideIndifferent(monster, perception);
                case MonsterState.Alert:
                    return DecideAlert(monster, perception);
                case MonsterState.Detected:
                    return DetectPlayer(monster);
                case MonsterState.Combat:
                    return DecideCombat(monster, perception);
                default:
                    return MonsterDecision.None(monster.State);
            }
        }

        private static MonsterDecision DecideIndifferent(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            if (action.Action == PlayerActionType.ThrowItem)
            {
                return MonsterDecision.None(monster.State);
            }

            if (IsHostileTargetedAction(perception))
            {
                return EnterCombat(monster, true);
            }

            if (IsDirectDiscoveryAction(perception))
            {
                return DetectPlayer(monster);
            }

            if (action.Action == PlayerActionType.Talk &&
                IsTargeted(perception))
            {
                monster.EnterAlert();
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Wait(),
                    monster.Name + "은(는) 주변을 경계하고 있다.");
            }

            if (action.Action == PlayerActionType.Wait ||
                action.Action == PlayerActionType.Defend)
            {
                return new MonsterDecision(
                    MonsterState.Indifferent,
                    MonsterActionPlan.Wait(),
                    monster.Name + "은(는) 아무 반응도 하지 않는다.");
            }

            return MonsterDecision.None(monster.State);
        }

        private static MonsterDecision DecideAlert(
            Monster monster,
            MonsterPerception perception)
        {
            if (perception.PlayerAction.Action ==
                PlayerActionType.ThrowItem)
            {
                return MonsterDecision.None(monster.State);
            }

            if (IsHostileTargetedAction(perception))
            {
                return EnterCombat(monster, true);
            }

            if (IsDirectDiscoveryAction(perception) ||
                perception.PlayerAction.Action == PlayerActionType.Talk &&
                IsTargeted(perception))
            {
                return DetectPlayer(monster);
            }

            if (monster.AdvanceDetection())
            {
                return DetectPlayer(monster);
            }

            return new MonsterDecision(
                MonsterState.Alert,
                MonsterActionPlan.Wait(),
                monster.Name + "와의 거리가 가까워지고 있다.");
        }

        private static MonsterDecision DecideCombat(
            Monster monster,
            MonsterPerception perception)
        {
            string message = GetCombatReactionMessage(
                monster,
                perception.PlayerAction.Action);
            SkillType rush = new SkillData().SkillTypes[RushSkillId];
            if (monster.WasHitSinceLastAction &&
                SkillCostResolver.CanPay(monster, rush))
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.UseSkill(RushSkillId),
                    message);
            }

            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Defend(),
                string.IsNullOrEmpty(message)
                    ? monster.Name + "은(는) 몸을 웅크리고 방어한다."
                    : message);
        }

        private static MonsterDecision DetectPlayer(Monster monster)
        {
            return new MonsterDecision(
                MonsterState.Detected,
                MonsterActionPlan.Attack(MonsterState.Combat),
                monster.Name + "은(는) 당신을 발견했다!");
        }

        private static MonsterDecision EnterCombat(
            Monster monster,
            bool announceDetection)
        {
            SkillType rush = new SkillData().SkillTypes[RushSkillId];
            MonsterActionPlan action =
                monster.WasHitSinceLastAction &&
                SkillCostResolver.CanPay(monster, rush)
                    ? MonsterActionPlan.UseSkill(RushSkillId)
                    : MonsterActionPlan.Defend();
            return new MonsterDecision(
                MonsterState.Combat,
                action,
                announceDetection
                    ? monster.Name + "은(는) 당신을 발견했다!"
                    : null);
        }

        private static bool IsDirectDiscoveryAction(
            MonsterPerception perception)
        {
            return IsTargeted(perception) &&
                   perception.PlayerAction.Action ==
                       PlayerActionType.Search ||
                   IsTargetedSkill(perception);
        }

        private static bool IsHostileTargetedAction(
            MonsterPerception perception)
        {
            return IsTargeted(perception) &&
                   perception.PlayerAction.Action ==
                       PlayerActionType.Attack ||
                   IsTargetedSkill(perception) &&
                   perception.PlayerAction.SkillUse.Skill.Effects.Exists(
                       effect => effect.Operation ==
                                 EffectOperation.Attack ||
                                 effect.Operation ==
                                 EffectOperation.Damage);
        }

        private static bool IsTargetedSkill(
            MonsterPerception perception)
        {
            SkillUseContext skillUse =
                perception.PlayerAction.SkillUse;
            return perception.PlayerAction.Action ==
                       PlayerActionType.UseSkill &&
                   skillUse != null &&
                   skillUse.Skill.TargetingType !=
                       SkillTargetingType.None &&
                   skillUse.SelectedTargets.Exists(
                       target => ReferenceEquals(target,
                           perception.CurrentSlot.Content));
        }

        private static bool IsTargeted(
            MonsterPerception perception)
        {
            return perception.PlayerAction.TargetSlot ==
                   perception.CurrentSlot;
        }

        private static string GetCombatReactionMessage(
            Monster monster,
            PlayerActionType action)
        {
            if (action == PlayerActionType.Wait)
            {
                return monster.Name + "은(는) 기다려주지 않는다.";
            }

            if (action == PlayerActionType.Search)
            {
                return monster.Name + "은(는) 손길을 뿌리쳤다.";
            }

            if (action == PlayerActionType.Talk)
            {
                return monster.Name + "은(는) 대답하지 않는다.";
            }

            return null;
        }
    }
}
