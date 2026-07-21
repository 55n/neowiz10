using System.Collections.Generic;

namespace Darkness
{
    public class EffectRequest
    {
        public IEnumerable<EffectApplication> Applications
        {
            get;
            private set;
        }
        public EffectContext Context { get; private set; }

        public EffectRequest(
            IEnumerable<EffectApplication> applications,
            EffectContext context)
        {
            Applications = applications;
            Context = context;
        }
    }
}
