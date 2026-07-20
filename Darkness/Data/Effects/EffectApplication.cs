namespace Darkness
{
    public class EffectApplication
    {
        public EffectOperation Operation { get; private set; }
        public string EffectId { get; private set; }
        public EffectTarget Target { get; private set; }
        public int ApplyChance { get; private set; }
        public int Magnitude { get; private set; }
        public int StackCount { get; private set; }
        public AttackDeliveryType AttackDeliveryType { get; private set; }
        public EquipmentSlot EquipmentSlot { get; private set; }
        public bool UsesFixedAttackDamage { get; private set; }
        public int AccuracyModifier { get; private set; }

        public EffectApplication(
            string effectId,
            EffectTarget target,
            int applyChance)
            : this(
                EffectOperation.ApplyStatus,
                effectId,
                target,
                applyChance,
                0,
                1,
                AttackDeliveryType.Natural,
                EquipmentSlot.Weapon,
                false,
                0)
        {
        }

        private EffectApplication(
            EffectOperation operation,
            string effectId,
            EffectTarget target,
            int applyChance,
            int magnitude,
            int stackCount,
            AttackDeliveryType attackDeliveryType,
            EquipmentSlot equipmentSlot,
            bool usesFixedAttackDamage,
            int accuracyModifier)
        {
            Operation = operation;
            EffectId = effectId;
            Target = target;
            ApplyChance = applyChance;
            Magnitude = magnitude;
            StackCount = stackCount;
            AttackDeliveryType = attackDeliveryType;
            EquipmentSlot = equipmentSlot;
            UsesFixedAttackDamage = usesFixedAttackDamage;
            AccuracyModifier = accuracyModifier;
        }

        public static EffectApplication ApplyStatus(
            string effectId,
            EffectTarget target,
            int applyChance = 100,
            int stackCount = 1)
        {
            return new EffectApplication(
                EffectOperation.ApplyStatus,
                effectId,
                target,
                applyChance,
                0,
                stackCount,
                AttackDeliveryType.Natural,
                EquipmentSlot.Weapon,
                false,
                0);
        }

        public static EffectApplication Damage(
            EffectTarget target,
            int amount,
            int applyChance = 100)
        {
            return new EffectApplication(
                EffectOperation.Damage,
                null,
                target,
                applyChance,
                amount,
                1,
                AttackDeliveryType.Natural,
                EquipmentSlot.Weapon,
                false,
                0);
        }

        public static EffectApplication Attack(
            EffectTarget target,
            int damagePercent,
            int applyChance = 100,
            AttackDeliveryType attackDeliveryType =
                AttackDeliveryType.Natural)
        {
            return new EffectApplication(
                EffectOperation.Attack,
                null,
                target,
                applyChance,
                damagePercent,
                1,
                attackDeliveryType,
                EquipmentSlot.Weapon,
                false,
                0);
        }

        public static EffectApplication FixedAttack(
            EffectTarget target,
            int damage,
            int accuracyModifier = 0,
            int applyChance = 100,
            AttackDeliveryType attackDeliveryType =
                AttackDeliveryType.Natural)
        {
            return new EffectApplication(
                EffectOperation.Attack,
                null,
                target,
                applyChance,
                damage,
                1,
                attackDeliveryType,
                EquipmentSlot.Weapon,
                true,
                accuracyModifier);
        }

        public static EffectApplication SetEquipmentDurability(
            string effectId,
            EffectTarget target,
            EquipmentSlot equipmentSlot,
            int durability,
            int applyChance = 100)
        {
            return new EffectApplication(
                EffectOperation.SetEquipmentDurability,
                effectId,
                target,
                applyChance,
                durability,
                1,
                AttackDeliveryType.Natural,
                equipmentSlot,
                false,
                0);
        }
    }
}
