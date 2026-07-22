using System.Collections.Generic;

namespace Darkness
{
    public class FrozenEffect : ActiveEffect
    {
        private const int IncomingDamageBonus = 1;

        public FrozenEffect(EffectType type, object source = null)
            : base(type, source)
        {
        }

        public override bool ModifyIncomingDamage(
            DamageContext context)
        {
            context.FinalDamage += IncomingDamageBonus;
            return false;
        }

        public override IEnumerable<string> GetTriggeredEffectIds(
            IEffectTarget target)
        {
            bool isWet = target != null &&
                         target.Effects != null &&
                         target.Effects.Exists(effect =>
                             effect.Type != null &&
                             effect.Type.Id == "wet");
            return isWet
                ? new[] { "bind" }
                : new string[0];
        }
    }
}
