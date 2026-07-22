using System.Collections.Generic;

namespace Darkness
{
    public class SkillUseResult
    {
        public bool Succeeded { get; set; }
        public List<string> Messages { get; private set; }
        public List<AttackContext> Attacks { get; private set; }
        public List<DamageContext> Damages { get; private set; }
        public List<MonsterMoveRequest> MonsterMoves { get; private set; }
        public List<EquipmentDurabilityRequest> DurabilityRequests
        {
            get;
            private set;
        }

        public SkillUseResult()
        {
            Messages = new List<string>();
            Attacks = new List<AttackContext>();
            Damages = new List<DamageContext>();
            MonsterMoves = new List<MonsterMoveRequest>();
            DurabilityRequests =
                new List<EquipmentDurabilityRequest>();
        }
    }
}
