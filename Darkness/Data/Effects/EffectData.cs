using System.Collections.Generic;

namespace Darkness
{
    public class EffectData
    {
        public Dictionary<string, EffectType> EffectTypes { get; private set; }

        public EffectData()
        {
            EffectType guardianBlessing = new EffectType(
                "guardian_blessing",
                "수호의 가호",
                "치명적인 피해를 받으면 체력을 1로 남긴다.",
                null,
                false,
                1);
            EffectType hasty = new EffectType(
                "hasty",
                "성급함",
                "서둘러 이동하는 동안 회피율이 0이 된다.",
                1,
                false,
                1);

            EffectTypes = new Dictionary<string, EffectType>
            {
                { guardianBlessing.Id, guardianBlessing },
                { hasty.Id, hasty }
            };
        }
    }
}
