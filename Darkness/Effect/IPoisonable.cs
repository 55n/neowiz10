namespace Darkness
{
    public interface IPoisonable : IDamageable, IEffectTarget
    {
        bool CanBePoisoned { get; }
    }
}
