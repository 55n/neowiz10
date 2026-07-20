namespace Darkness
{
    public interface ISkillUser
    {
        string Name { get; }
        int CurrentFocus { get; }
        int Attack { get; }
        int Accuracy { get; }
        Inventory Inventory { get; }

        bool KnowsSkill(string skillId);
        void SpendFocus(int amount);
        void RestoreFocus(int amount);
    }
}
