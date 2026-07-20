using System.Collections.Generic;

namespace Darkness
{
    public class EffectPlan
    {
        internal object Source { get; private set; }
        internal List<PlannedEffect> Effects { get; private set; }

        internal EffectPlan(object source)
        {
            Source = source;
            Effects = new List<PlannedEffect>();
        }
    }

    internal class PlannedEffect
    {
        public EffectApplication Application { get; private set; }
        public object Target { get; private set; }
        public ActiveEffect ActiveEffect { get; private set; }

        public PlannedEffect(
            EffectApplication application,
            object target,
            ActiveEffect activeEffect)
        {
            Application = application;
            Target = target;
            ActiveEffect = activeEffect;
        }
    }
}
