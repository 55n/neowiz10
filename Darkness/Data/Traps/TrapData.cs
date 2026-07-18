using System;
using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class TrapData
    {
        public Dictionary<string, TrapType> TrapTypes { get; private set; }

        public TrapData()
        {
            TrapTypes = new List<TrapType>
            {
                new TrapType(
                    "arrow_trap",
                    "화살 함정",
                    "공격하거나 손으로 더듬으면 벽에서 화살이 발사된다.",
                    5,
                    85,
                    true,
                    new List<EffectApplication>())
            }.ToDictionary(trapType => trapType.Id);
        }
    }
}
