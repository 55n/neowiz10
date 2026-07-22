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
                    "화살함정",
                    "공격하거나 손으로 더듬으면 벽에서 화살이 발사된다.",
                    5,
                    0,
                    5,
                    85,
                    true,
                    new List<EffectApplication>()),
                new TrapType(
                    "poison_fog_trap",
                    "독안개",
                    "바닥의 장치에서 맹독안개가 계속 뿜어져 나온다.",
                    8,
                    0,
                    0,
                    100,
                    false,
                    new List<EffectApplication>
                    {
                        EffectApplication.ApplyStatus(
                            "poison",
                            EffectTarget.AllRoomOccupants)
                    })
            }.ToDictionary(trapType => trapType.Id);
        }
    }
}
