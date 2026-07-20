namespace Darkness
{
    public class HastyEffect : ActiveEffect
    {
        public HastyEffect(EffectType type)
            : base(type)
        {
        }

        public override bool ModifyIncomingAttack(AttackContext context)
        {
            context.Evasion = 0;
            return false;
        }
    }
}
