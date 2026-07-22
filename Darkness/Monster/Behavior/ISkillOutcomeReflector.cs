namespace Darkness
{
    public interface ISkillOutcomeReflector
    {
        bool HasPendingSkillOutcome { get; }
        void BeginSkillResolution(Monster monster);
        void CompleteSkillResolution(Monster monster);
    }
}
