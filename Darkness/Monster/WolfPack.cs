using System.Collections.Generic;

namespace Darkness
{
    public class WolfPack
    {
        private readonly List<Monster> members;
        private int observedLivingCount;
        private bool tacticsAnnounced;

        public WolfPack()
        {
            members = new List<Monster>();
        }

        public void Add(Monster wolf)
        {
            if (wolf != null && !members.Contains(wolf))
            {
                members.Add(wolf);
                observedLivingCount = GetLivingMemberCount();
            }
        }

        public bool TryAnnounceTactics()
        {
            if (tacticsAnnounced)
            {
                return false;
            }

            tacticsAnnounced = true;
            return true;
        }

        public bool TryAnnounceLoss(out int livingCount)
        {
            livingCount = GetLivingMemberCount();
            if (livingCount >= observedLivingCount)
            {
                return false;
            }

            observedLivingCount = livingCount;
            return true;
        }

        public int GetLivingAllyCount(Monster owner)
        {
            if (owner == null)
            {
                return 0;
            }

            int count = 0;
            foreach (Monster member in members)
            {
                if (member != null && member.IsAlive &&
                    !ReferenceEquals(member, owner) &&
                    member.Id == owner.Id)
                {
                    count++;
                }
            }

            return count;
        }

        private int GetLivingMemberCount()
        {
            int count = 0;
            foreach (Monster member in members)
            {
                if (member != null && member.IsAlive)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
