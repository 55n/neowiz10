namespace Darkness
{
    public class TrapMisfireEffect : ActiveEffect
    {
        public TrapMisfireEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyOutgoingAttack(
            AttackContext context)
        {
            if (context.DeliveryType != AttackDeliveryType.Trap)
            {
                return false;
            }

            context.Accuracy = 0;
            return true;
        }
    }
}
