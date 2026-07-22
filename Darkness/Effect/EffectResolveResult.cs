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
        public List<AppliedEffectResult> AppliedEffects
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
            AppliedEffects = new List<AppliedEffectResult>();
        }
    }

    public class AppliedEffectResult
    {
        public IEffectTarget Target { get; private set; }
        public EffectType Effect { get; private set; }
        public int StackCount { get; private set; }
        public bool WasTriggered { get; private set; }

        public AppliedEffectResult(
            IEffectTarget target,
            EffectType effect,
            int stackCount,
            bool wasTriggered)
        {
            Target = target;
            Effect = effect;
            StackCount = stackCount;
            WasTriggered = wasTriggered;
        }
    }
}
