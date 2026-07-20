using System.Collections.Generic;

namespace Darkness
{
    public class DurabilityResolveResult
    {
        public List<string> Messages { get; private set; }

        public DurabilityResolveResult()
        {
            Messages = new List<string>();
        }
    }
}
