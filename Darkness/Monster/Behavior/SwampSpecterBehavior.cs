using System;
using System.Collections.Generic;

namespace Darkness
{
    public class SwampSpecterBehavior :
        IMonsterBehavior,
        ISlotMovementBehavior
    {
        private static readonly Random Random = new Random();
        private bool isPreparingAmbush;

        public MonsterDecision Decide(
            Monster monster,
            MonsterPerception perception)
        {
            PlayerActionContext action = perception.PlayerAction;
            bool isDirectlyProvoked =
                action != null &&
                action.TargetSlot == perception.CurrentSlot &&
                (action.Action == PlayerActionType.Attack ||
                 action.Action == PlayerActionType.ThrowItem ||
                 action.Action == PlayerActionType.Search);
            bool isCombat = isDirectlyProvoked ||
                monster.State == MonsterState.Detected ||
                monster.State == MonsterState.Combat;

            if (!isCombat)
            {
                MonsterState nextState = IsSuspiciousAction(action)
                    ? MonsterState.Alert
                    : monster.State;
                RoomSlot roamingTarget = FindAdjacentWater(perception);
                if (roamingTarget != null)
                {
                    return new MonsterDecision(
                        nextState,
                        MonsterActionPlan.MoveTo(
                            roamingTarget,
                            nextState),
                        NarrativeTokens.Actor +
                        "이(가) 물속에서 느릿하게 자리를 옮겼다.");
                }

                return new MonsterDecision(
                    nextState,
                    MonsterActionPlan.Wait(),
                    "물결이 잦아들고 수면이 잠잠해졌다.");
            }

            if (monster.CurrentFocus <= 0)
            {
                isPreparingAmbush = false;
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.Attack(MonsterState.Combat),
                    NarrativeTokens.Actor + "이(가) 물속에서 몸을 일으켰다.");
            }

            RoomSlot target = FindAdjacentWater(perception);
            if (!isPreparingAmbush)
            {
                isPreparingAmbush = true;
                MonsterActionPlan preparationAction = target == null
                    ? MonsterActionPlan.Wait()
                    : MonsterActionPlan.MoveTo(
                        target,
                        MonsterState.Combat);
                return new MonsterDecision(
                    MonsterState.Combat,
                    preparationAction,
                    NarrativeTokens.Actor +
                    "이(가) 깊이 가라앉자 수면 위의 그림자가 사라졌다.");
            }

            isPreparingAmbush = false;
            if (target != null)
            {
                return new MonsterDecision(
                    MonsterState.Combat,
                    MonsterActionPlan.UseSkill(
                        "waterside_ambush",
                        target),
                    "발밑의 물이 갑자기 거칠게 솟구쳤다.");
            }

            return new MonsterDecision(
                MonsterState.Combat,
                MonsterActionPlan.Attack(MonsterState.Combat),
                NarrativeTokens.Actor +
                "이(가) 빠져나갈 물길을 찾지 못하고 달려들었다.");
        }

        private static bool IsSuspiciousAction(PlayerActionContext action)
        {
            return action != null &&
                (action.Action == PlayerActionType.Move ||
                 action.Action == PlayerActionType.Search ||
                 action.Action == PlayerActionType.Talk ||
                 action.Action == PlayerActionType.ThrowItem);
        }

        public bool CanEnter(ISlotContent content)
        {
            return content is Water;
        }

        public ISlotContent CreateContentForVacatedSlot(RoomSlot slot)
        {
            string id = slot == null || slot.Type == null
                ? "swamp_specter_water"
                : slot.Type.ObjectTypeId + "_water";
            return new Water(id);
        }

        private static RoomSlot FindAdjacentWater(
            MonsterPerception perception)
        {
            if (perception == null || perception.CurrentRoom == null ||
                perception.CurrentSlot == null)
            {
                return null;
            }

            List<RoomSlot> slots = perception.CurrentRoom.Slots;
            int currentIndex = slots.IndexOf(perception.CurrentSlot);
            List<RoomSlot> candidates = new List<RoomSlot>();
            AddWaterAt(slots, currentIndex - 1, candidates);
            AddWaterAt(slots, currentIndex + 1, candidates);
            return candidates.Count == 0
                ? null
                : candidates[Random.Next(candidates.Count)];
        }

        private static void AddWaterAt(
            List<RoomSlot> slots,
            int index,
            List<RoomSlot> candidates)
        {
            if (index >= 0 && index < slots.Count &&
                slots[index].Content is Water)
            {
                candidates.Add(slots[index]);
            }
        }
    }
}
