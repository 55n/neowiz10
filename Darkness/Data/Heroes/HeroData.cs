using System.Collections.Generic;

namespace Darkness
{
    public class HeroData
    {
        public Dictionary<string, HeroType> HeroTypes { get; private set; }

        public HeroData()
        {
            HeroType explorer = new HeroType(
                "hero",
                "탐험가",
                "어둠 속을 헤매는 탐험가다.",
                20,
                8,
                5,
                2,
                80,
                15);
            HeroTypes = new Dictionary<string, HeroType>
            {
                { explorer.Id, explorer }
            };
        }
    }
}
