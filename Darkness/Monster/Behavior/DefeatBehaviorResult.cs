namespace Darkness
{
    public class DefeatBehaviorResult
    {
        public bool PreventRemoval { get; private set; }
        public string Message { get; private set; }

        private DefeatBehaviorResult(
            bool preventRemoval,
            string message)
        {
            PreventRemoval = preventRemoval;
            Message = message;
        }

        public static DefeatBehaviorResult Continue(string message)
        {
            return new DefeatBehaviorResult(true, message);
        }

        public static DefeatBehaviorResult Complete()
        {
            return new DefeatBehaviorResult(false, null);
        }
    }
}
