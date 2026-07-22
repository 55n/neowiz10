using System;

namespace Darkness
{
    public class HungryTrollBehavior :
        IMonsterBehavior,
        IBaitResponsiveBehavior
    {
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

            if (IsBaitThrown(perception) &&
                MonsterBaitTargeting.IsNearestResponsiveMonster(
                    perception))
            {
                return FollowBait(monster, perception);
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
                    return DecideCombat(monster);
                default:
                    return MonsterDecision.None(monster.State);
            }
        }

        private static MonsterDecision DecideIndifferent(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionType action = perception.PlayerAction.Action;
            if (action == PlayerActionType.Defend ||
                action == PlayerActionType.Move ||
                IsUntargetedSkill(perception))
            {
                return MonsterDecision.None(monster.State);
            }

            if (action == PlayerActionType.ThrowItem)
            {
                return EnterAlert(
                    monster,
                    NarrativeTokens.Actor +
                    "은(는) 물건이 떨어진 소리에 경계한다.");
            }

            if (perception.LoudEventOccurred)
            {
                return EnterAlert(
                    monster,
                    NarrativeTokens.Actor + "은(는) 큰 소리에 반응한다.");
            }

            if (action == PlayerActionType.Wait)
            {
                return EnterAlert(
                    monster,
                    NarrativeTokens.Actor +
                    "이(가) 당신의 냄새를 맡은 것 같다.");
            }

            if (IsDirectTargetAction(perception))
            {
                return DetectPlayer(monster);
            }

            if (action == PlayerActionType.Talk)
            {
                return EnterAlert(
                    monster,
                    NarrativeTokens.Actor +
                    "이(가) 목소리가 들린 방향을 바라본다.");
            }

            return MonsterDecision.None(monster.State);
        }

        private static MonsterDecision DecideAlert(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionType action = perception.PlayerAction.Action;
            if (action == PlayerActionType.Defend ||
                IsUntargetedSkill(perception) ||
                action == PlayerActionType.Search &&
                !IsTargeted(perception) &&
                !perception.LoudEventOccurred)
            {
                return MonsterDecision.None(MonsterState.Alert);
            }

            if (action == PlayerActionType.Move)
            {
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Attack(),
                    NarrativeTokens.Actor +
                    "은(는) 떠나려는 당신을 공격한다.");
            }

            if (action == PlayerActionType.ThrowItem)
            {
                monster.EnterAlert();
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Wait(),
                    NarrativeTokens.Actor +
                    "은(는) 물건이 떨어진 곳을 경계한다.");
            }

            if (perception.LoudEventOccurred)
            {
                monster.EnterAlert();
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Wait(),
                    NarrativeTokens.Actor + "은(는) 큰 소리에 반응한다.");
            }

            if (action == PlayerActionType.Wait)
            {
                if (monster.AdvanceDetection())
                {
                    return DetectPlayer(monster);
                }

                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Wait(),
                    NarrativeTokens.Actor + "와의 거리가 가까워지고 있다. " +
                    monster.DetectionTurnsRemaining + "턴 남음.");
            }

            if (IsDirectTargetAction(perception))
            {
                return DetectPlayer(monster);
            }

            return MonsterDecision.None(MonsterState.Alert);
        }

        private static MonsterDecision DetectPlayer(Monster monster)
        {
            return new MonsterDecision(
                MonsterState.Detected,
                MonsterActionPlan.Attack(MonsterState.Combat),
                NarrativeTokens.Actor + "은(는) 당신을 발견했다.");
        }

        private static MonsterDecision DecideCombat(Monster monster)
        {
            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(),
                NarrativeTokens.Actor + "은(는) 당신을 기다려주지 않는다.");
        }

        private static MonsterDecision EnterAlert(
            Monster monster,
            string message)
        {
            monster.EnterAlert();
            return new MonsterDecision(
                MonsterState.Alert,
                MonsterActionPlan.Wait(),
                message);
        }

        private static bool IsDirectTargetAction(
            MonsterPerception perception)
        {
            PlayerActionType action = perception.PlayerAction.Action;
            return IsTargeted(perception) &&
                       (action == PlayerActionType.Attack ||
                        action == PlayerActionType.Search ||
                        action == PlayerActionType.Talk) ||
                   IsTargetedSkill(perception);
        }

        private static bool IsUntargetedSkill(
            MonsterPerception perception)
        {
            return perception.PlayerAction.Action ==
                       PlayerActionType.UseSkill &&
                   !IsTargetedSkill(perception);
        }

        private static bool IsTargetedSkill(
            MonsterPerception perception)
        {
            SkillUseContext skillUse = perception.PlayerAction.SkillUse;
            return perception.PlayerAction.Action ==
                       PlayerActionType.UseSkill &&
                   skillUse != null &&
                   skillUse.SelectedTargets.Exists(
                       target => ReferenceEquals(
                           target,
                           perception.CurrentSlot.Content));
        }

        private static bool IsTargeted(MonsterPerception perception)
        {
            return perception.PlayerAction.TargetSlot ==
                   perception.CurrentSlot;
        }

        private static bool IsBaitThrown(MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            return action.Action == PlayerActionType.ThrowItem &&
                   action.Item != null &&
                   action.Item.Item != null &&
                   action.Item.Item.Type.ThrowFunction == ItemFunction.Lure;
        }

        private static MonsterDecision FollowBait(
            Monster monster,
            MonsterPerception perception)
        {
            monster.EnterAlert();
            if (ReferenceEquals(
                perception.PlayerAction.TargetSlot,
                perception.CurrentSlot))
            {
                return new MonsterDecision(
                    MonsterState.Indifferent,
                    MonsterActionPlan.Wait(),
                    NarrativeTokens.Actor +
                    "은(는) 미끼에 정신이 팔렸다.");
            }

            RoomSlot destination = FindBaitDestination(
                perception.CurrentRoom,
                perception.CurrentSlot,
                perception.PlayerAction.TargetSlot);
            if (destination == null)
            {
                return new MonsterDecision(
                    MonsterState.Alert,
                    MonsterActionPlan.Wait(),
                    NarrativeTokens.Actor +
                    "은(는) 미끼 냄새가 나는 방향을 살핀다.");
            }

            return new MonsterDecision(
                MonsterState.Alert,
                MonsterActionPlan.MoveTo(
                    destination,
                    MonsterState.Indifferent),
                NarrativeTokens.Actor + "은(는) 미끼 냄새를 따라 움직인다.");
        }

        private static RoomSlot FindBaitDestination(
            Room room,
            RoomSlot currentSlot,
            RoomSlot baitSlot)
        {
            if (room == null || baitSlot == null ||
                ReferenceEquals(currentSlot, baitSlot))
            {
                return null;
            }

            if (baitSlot.IsEmpty)
            {
                return baitSlot;
            }

            int baitIndex = room.Slots.IndexOf(baitSlot);
            int currentIndex = room.Slots.IndexOf(currentSlot);
            RoomSlot nearestSlot = null;
            int nearestBaitDistance = int.MaxValue;
            int nearestTrollDistance = int.MaxValue;

            for (int i = 0; i < room.Slots.Count; i++)
            {
                RoomSlot candidate = room.Slots[i];
                if (!candidate.IsEmpty)
                {
                    continue;
                }

                int baitDistance = Math.Abs(i - baitIndex);
                int trollDistance = Math.Abs(i - currentIndex);
                if (baitDistance < nearestBaitDistance ||
                    baitDistance == nearestBaitDistance &&
                    trollDistance < nearestTrollDistance)
                {
                    nearestSlot = candidate;
                    nearestBaitDistance = baitDistance;
                    nearestTrollDistance = trollDistance;
                }
            }

            return nearestSlot;
        }
    }
}
