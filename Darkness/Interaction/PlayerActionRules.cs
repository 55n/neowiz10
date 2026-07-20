namespace Darkness
{
    public static class PlayerActionRules
    {
        public static bool RequiresTargetSlot(
            PlayerActionType action)
        {
            return action == PlayerActionType.Talk ||
                   action == PlayerActionType.ThrowItem ||
                   action == PlayerActionType.Search ||
                   action == PlayerActionType.Attack;
        }
    }
}
