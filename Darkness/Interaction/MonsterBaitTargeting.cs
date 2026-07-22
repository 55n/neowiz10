using System;

namespace Darkness
{
    public static class MonsterBaitTargeting
    {
        public static bool IsNearestResponsiveMonster(
            MonsterPerception perception)
        {
            if (perception == null ||
                perception.CurrentRoom == null ||
                perception.CurrentSlot == null ||
                perception.PlayerAction == null ||
                perception.PlayerAction.TargetSlot == null)
            {
                return false;
            }

            int targetIndex = perception.CurrentRoom.Slots.IndexOf(
                perception.PlayerAction.TargetSlot);
            if (targetIndex < 0)
            {
                return false;
            }

            Monster nearest = null;
            int nearestDistance = int.MaxValue;
            for (int slotIndex = 0;
                 slotIndex < perception.CurrentRoom.Slots.Count;
                 slotIndex++)
            {
                Monster candidate =
                    perception.CurrentRoom.Slots[slotIndex].Content
                    as Monster;
                if (candidate == null || !candidate.CanAct ||
                    !(candidate.Behavior is IBaitResponsiveBehavior))
                {
                    continue;
                }

                int distance = Math.Abs(slotIndex - targetIndex);
                if (distance < nearestDistance)
                {
                    nearest = candidate;
                    nearestDistance = distance;
                }
            }

            return nearest != null && ReferenceEquals(
                nearest,
                perception.CurrentSlot.Content);
        }
    }
}
