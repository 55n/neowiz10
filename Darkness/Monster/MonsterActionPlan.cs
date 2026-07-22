using System.Collections.Generic;

namespace Darkness
{
    public class MonsterActionPlan
    {
        public MonsterActionType Type { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public string SkillId { get; private set; }
        public MonsterState? StateAfterAction { get; private set; }
        public int ReflectedDamage { get; private set; }
        public List<ReflectedEffect> ReflectedEffects { get; private set; }

        private MonsterActionPlan(
            MonsterActionType type,
            RoomSlot targetSlot,
            string skillId,
            MonsterState? stateAfterAction)
        {
            Type = type;
            TargetSlot = targetSlot;
            SkillId = skillId;
            StateAfterAction = stateAfterAction;
            ReflectedEffects = new List<ReflectedEffect>();
        }

        public static MonsterActionPlan None()
        {
            return new MonsterActionPlan(
                MonsterActionType.None, null, null, null);
        }

        public static MonsterActionPlan Wait()
        {
            return new MonsterActionPlan(
                MonsterActionType.Wait, null, null, null);
        }

        public static MonsterActionPlan Attack(
            MonsterState? stateAfterAction = null)
        {
            return new MonsterActionPlan(
                MonsterActionType.Attack,
                null,
                null,
                stateAfterAction);
        }

        public static MonsterActionPlan MoveTo(
            RoomSlot targetSlot,
            MonsterState? stateAfterAction = null)
        {
            return new MonsterActionPlan(
                MonsterActionType.Move,
                targetSlot,
                null,
                stateAfterAction);
        }

        public static MonsterActionPlan Defend()
        {
            return new MonsterActionPlan(
                MonsterActionType.Defend, null, null, null);
        }

        public static MonsterActionPlan UseSkill(
            string skillId,
            RoomSlot followUpSlot = null)
        {
            return new MonsterActionPlan(
                MonsterActionType.UseSkill,
                followUpSlot,
                skillId,
                null);
        }

        public static MonsterActionPlan DrainFocus()
        {
            return new MonsterActionPlan(
                MonsterActionType.DrainFocus,
                null,
                null,
                MonsterState.Combat);
        }

        public static MonsterActionPlan Reflect(
            int damage,
            IEnumerable<ReflectedEffect> effects)
        {
            MonsterActionPlan plan = new MonsterActionPlan(
                MonsterActionType.Reflect,
                null,
                null,
                MonsterState.Combat);
            plan.ReflectedDamage = System.Math.Max(0, damage);
            if (effects != null)
            {
                plan.ReflectedEffects.AddRange(effects);
            }

            return plan;
        }

        public static MonsterActionPlan Vanish()
        {
            return new MonsterActionPlan(
                MonsterActionType.Vanish,
                null,
                null,
                null);
        }
    }
}
