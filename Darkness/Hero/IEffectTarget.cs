namespace Darkness
{
    public interface IEffectTarget
    {
        void ApplyEffect(ActiveEffect effect);
        void RemoveEffect(string effectId);
    }
}
