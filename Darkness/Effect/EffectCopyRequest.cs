namespace Darkness
{
    public class EffectCopyRequest
    {
        public object Source { get; private set; }
        public IEffectTarget Target { get; private set; }
        public string EffectId { get; private set; }
        public int StackCount { get; private set; }

        public EffectCopyRequest(
            object source,
            IEffectTarget target,
            string effectId,
            int stackCount)
        {
            Source = source;
            Target = target;
            EffectId = effectId;
            StackCount = stackCount;
        }
    }
}
