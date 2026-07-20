using System;

namespace Darkness
{
    public class AttackContext
    {
        public IDamageable Source { get; private set; }
        public IDamageable Target { get; private set; }
        public int BaseDamage { get; private set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
        public AttackDeliveryType DeliveryType { get; private set; }
        public Item UsedItem { get; private set; }
        public int UsedItemDurabilityCost { get; private set; }

        public AttackContext(
            IDamageable source,
            IDamageable target,
            int baseDamage,
            int accuracy,
            int evasion)
            : this(
                source,
                target,
                baseDamage,
                accuracy,
                evasion,
                AttackDeliveryType.Natural,
                null,
                0)
        {
        }

        public AttackContext(
            IDamageable source,
            IDamageable target,
            int baseDamage,
            int accuracy,
            int evasion,
            AttackDeliveryType deliveryType,
            Item usedItem,
            int usedItemDurabilityCost)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            Source = source;
            Target = target;
            BaseDamage = Math.Max(0, baseDamage);
            Accuracy = accuracy;
            Evasion = evasion;
            DeliveryType = deliveryType;
            UsedItem = usedItem;
            UsedItemDurabilityCost = Math.Max(
                0,
                usedItemDurabilityCost);
        }
    }
}
