namespace Darkness
{
    public class ReflectedEffect
    {
        public string EffectId { get; private set; }
        public int StackCount { get; private set; }

        public ReflectedEffect(string effectId, int stackCount)
        {
            EffectId = effectId;
            StackCount = stackCount;
        }
    }
}
