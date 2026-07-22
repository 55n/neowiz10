namespace Darkness
{
    public interface ISearchHintBehavior
    {
        string GetSearchHint(
            Monster monster,
            PlayerActionContext context);
    }
}
