using System.Collections.Generic;

namespace Darkness
{
    public class MagicChargeEffect : ActiveEffect
    {
        private const string MagicOverloadEffectId =
            "magic_overload";

        public MagicChargeEffect(
            EffectType type,
            object source = null)
            : base(type, source)
        {
        }

        public override bool CanApplyTo(IEffectTarget target)
        {
            return target != null && target.Effects != null &&
                   !target.Effects.Exists(effect =>
                       effect != null && effect.Type != null &&
                       effect.Type.Id == "magic_stone_eater");
        }

        public override IEnumerable<string> GetTriggeredEffectIds(
            IEffectTarget target)
        {
            return new[] { MagicOverloadEffectId };
        }
    }
}
