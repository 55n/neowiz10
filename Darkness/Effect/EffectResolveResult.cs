using System.Collections.Generic;

namespace Darkness
{
    public class EffectResolveResult
    {
        public List<AttackContext> Attacks { get; private set; }
        public List<DamageContext> Damages { get; private set; }
        public List<EquipmentDurabilityRequest> DurabilityRequests
        {
            get;
            private set;
        }
        public HashSet<IEffectTarget> AffectedEffectTargets
        {
            get;
            private set;
        }

        public EffectResolveResult()
        {
            Attacks = new List<AttackContext>();
            Damages = new List<DamageContext>();
            DurabilityRequests =
                new List<EquipmentDurabilityRequest>();
            AffectedEffectTargets = new HashSet<IEffectTarget>();
        }
    }
}
