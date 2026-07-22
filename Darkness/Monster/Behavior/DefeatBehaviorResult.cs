namespace Darkness
{
    public class DefeatBehaviorResult
    {
        public bool PreventRemoval { get; private set; }
        public string Message { get; private set; }
        public ISlotContent ReplacementContent { get; private set; }

        private DefeatBehaviorResult(
            bool preventRemoval,
            string message,
            ISlotContent replacementContent)
        {
            PreventRemoval = preventRemoval;
            Message = message;
            ReplacementContent = replacementContent;
        }

        public static DefeatBehaviorResult Continue(string message)
        {
            return new DefeatBehaviorResult(true, message, null);
        }

        public static DefeatBehaviorResult Complete()
        {
            return new DefeatBehaviorResult(false, null, null);
        }

        public static DefeatBehaviorResult Complete(string message)
        {
            return new DefeatBehaviorResult(false, message, null);
        }

        public static DefeatBehaviorResult ReplaceWith(
            ISlotContent replacementContent,
            string message)
        {
            return new DefeatBehaviorResult(
                false,
                message,
                replacementContent);
        }
    }
}
