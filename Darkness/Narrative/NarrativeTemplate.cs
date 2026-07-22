namespace Darkness
{
    public static class NarrativeTokens
    {
        public const string Actor = "{actor}";
        public const string Target = "{target}";
    }

    public static class NarrativeTemplate
    {
        public static string Format(
            string template,
            string actor = null,
            string target = null)
        {
            if (string.IsNullOrEmpty(template))
            {
                return template;
            }

            return template
                .Replace(NarrativeTokens.Actor, actor ?? "")
                .Replace(NarrativeTokens.Target, target ?? "");
        }
    }
}
