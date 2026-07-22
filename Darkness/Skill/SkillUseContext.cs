using System;
using System.Collections.Generic;

namespace Darkness
{
    public class SkillUseContext
    {
        public ISkillUser User { get; private set; }
        public ISkillUser Source { get { return User; } }
        public SkillType Skill { get; private set; }
        public Room Room { get; private set; }
        public List<object> SelectedTargets { get; private set; }
        public List<object> Targets { get { return SelectedTargets; } }
        public RoomSlot FollowUpSlot { get; private set; }

        public SkillUseContext(
            ISkillUser user,
            SkillType skill,
            Room room,
            IEnumerable<object> selectedTargets,
            RoomSlot followUpSlot = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (skill == null)
            {
                throw new ArgumentNullException("skill");
            }

            User = user;
            Skill = skill;
            Room = room;
            SelectedTargets = selectedTargets == null
                ? new List<object>()
                : new List<object>(selectedTargets);
            FollowUpSlot = followUpSlot;
        }
    }
}
