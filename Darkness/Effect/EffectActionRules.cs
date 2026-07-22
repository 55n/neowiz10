namespace Darkness
{
    public static class EffectActionRules
    {
        public static bool HasForcedWait(IEffectTarget target)
        {
            return FindForcedWaitEffect(target) != null;
        }

        public static bool TryConsumeForcedWait(
            IEffectTarget target,
            out string effectName,
            out int remainingStacks)
        {
            effectName = null;
            remainingStacks = 0;

            ActiveEffect effect = FindForcedWaitEffect(target);
            if (effect == null)
            {
                return false;
            }

            effectName = effect.Type.Name;
            bool depleted = effect.ConsumeStack();
            remainingStacks = effect.StackCount;
            if (depleted)
            {
                target.Effects.Remove(effect);
            }

            return true;
        }

        private static ActiveEffect FindForcedWaitEffect(
            IEffectTarget target)
        {
            return target == null || target.Effects == null
                ? null
                : target.Effects.Find(effect =>
                    effect != null && effect.ForcesWait);
        }
    }
}
