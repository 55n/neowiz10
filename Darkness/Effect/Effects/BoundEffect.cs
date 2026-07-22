namespace Darkness
{
    public class BoundEffect : ActiveEffect
    {
        public override bool ForcesWait
        {
            get { return true; }
        }

        public BoundEffect(EffectType type, object source = null)
            : base(type, source)
        {
        }
    }
}
