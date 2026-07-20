using System;
using System.Collections.Generic;

namespace Darkness
{
    public class EffectContext
    {
        public object Source { get; private set; }
        public EffectOriginType OriginType { get; private set; }
        public string OriginId { get; private set; }
        public Room Room { get; private set; }
        public List<object> SelectedTargets { get; private set; }

        public EffectContext(
            object source,
            EffectOriginType originType,
            string originId,
            Room room,
            IEnumerable<object> selectedTargets)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            Source = source;
            OriginType = originType;
            OriginId = originId;
            Room = room;
            SelectedTargets = selectedTargets == null
                ? new List<object>()
                : new List<object>(selectedTargets);
        }

        public static EffectContext FromSkill(SkillUseContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new EffectContext(
                context.User,
                EffectOriginType.Skill,
                context.Skill.Id,
                context.Room,
                context.SelectedTargets);
        }

        public static EffectContext FromItemThrow(
            ItemThrowPlan plan,
            Room room)
        {
            if (plan == null)
            {
                throw new ArgumentNullException("plan");
            }

            return new EffectContext(
                plan.Thrower,
                EffectOriginType.ItemThrow,
                plan.ThrownItem.Type.Id,
                room,
                plan.Target == null
                    ? null
                    : new object[] { plan.Target });
        }
    }
}
