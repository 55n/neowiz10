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
            EffectType perfectFocus = new EffectType(
                "perfect_focus",
                "완벽한 집중",
                "다음 공격의 정확도에 100을 더한다.",
                null,
                false,
                1);
            EffectType defending = new EffectType(
                "defending",
                "방어",
                "몬스터 행동 동안 방어력이 3 증가한다.",
                1,
                false,
                1);
            EffectType trapMisfire = new EffectType(
                "trap_misfire",
                "오작동",
                "투척물에 의해 잘못 발동해 다음 공격이 빗나간다.",
                null,
                false,
                1);
            EffectType weaponEmpowerment = new EffectType(
                "weapon_empowerment",
                "무기 강화",
                "이 무기의 공격력을 2배로 증가시킨다.",
                null,
                false,
                1);
            EffectType startled = new EffectType(
                "startled",
                "깜짝 놀람",
                "다음 행동까지 정확도와 회피율이 10 감소한다.",
                1,
                false,
                1);
            EffectType poison = new EffectType(
                "poison",
                "중독",
                "매 플레이어 턴 종료 시 5의 피해를 입는다.",
                null,
                false,
                1);

            EffectTypes = new Dictionary<string, EffectType>
            {
                { guardianBlessing.Id, guardianBlessing },
                { hasty.Id, hasty },
                { perfectFocus.Id, perfectFocus },
                { defending.Id, defending },
                { trapMisfire.Id, trapMisfire },
                { weaponEmpowerment.Id, weaponEmpowerment },
                { startled.Id, startled },
                { poison.Id, poison }
            };
        }
    }
}
