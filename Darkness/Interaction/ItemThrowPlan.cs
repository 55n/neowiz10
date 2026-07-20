using System.Collections.Generic;

namespace Darkness
{
    public class ItemThrowPlan
    {
        public Hero Thrower { get; private set; }
        public Item ThrownItem { get; private set; }
        public RoomSlot TargetSlot { get; private set; }
        public IDamageable Target { get; private set; }
        public AttackContext ImpactAttack { get; private set; }
        public List<EffectApplication> OnHitEffects { get; private set; }

        public ItemThrowPlan(
            Hero thrower,
            Item thrownItem,
            RoomSlot targetSlot,
            IDamageable target,
            AttackContext impactAttack,
            IEnumerable<EffectApplication> onHitEffects)
        {
            Thrower = thrower;
            ThrownItem = thrownItem;
            TargetSlot = targetSlot;
            Target = target;
            ImpactAttack = impactAttack;
            OnHitEffects = onHitEffects == null
                ? new List<EffectApplication>()
                : new List<EffectApplication>(onHitEffects);
        }
    }
}
