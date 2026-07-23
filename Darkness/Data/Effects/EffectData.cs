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
                "매 플레이어 턴 종료 시 3의 피해를 입는다.",
                null,
                false,
                1);
            EffectType wet = new EffectType(
                "wet",
                "젖음",
                "회피율이 10 감소하고 얼어붙음으로 받는 추가 피해가 1 증가한다.",
                null,
                false,
                1);
            EffectType frozen = new EffectType(
                "frozen",
                "얼어붙음",
                "받는 모든 피해가 1 증가한다.",
                null,
                false,
                1);
            EffectType bind = new EffectType(
                "bind",
                "속박",
                "행동할 때 1스택을 소모하고 해당 행동을 강제로 기다리기로 바꾼다.",
                null,
                true,
                99);
            EffectType magicCharge = new EffectType(
                "magic_charge",
                "마력충전",
                "대상에게 마력부하 1스택을 부여한다.",
                null,
                true,
                99);
            EffectType magicOverload = new EffectType(
                "magic_overload",
                "마력부하",
                "10스택에 도달하면 폭발하여 9999의 피해를 받는다.",
                null,
                true,
                10);
            EffectType magicStoneEater = new EffectType(
                "magic_stone_eater",
                "마석섭취자",
                "마석을 흡수하므로 마력부하가 쌓이지 않는다.",
                null,
                false,
                1);
            EffectType fixedEffect = new EffectType(
                "fixed",
                "고정됨",
                "몸이 한자리에 고정되어 회피율이 -100이 된다.",
                null,
                false,
                1);
            EffectType packTactics = new EffectType(
                "pack_tactics",
                "무리전술",
                "방에 있는 다른 같은 종류의 몬스터 하나당 공격력과 방어력이 1 증가한다.",
                null,
                false,
                1);
            EffectType spiritBarrier = new EffectType(
                "spirit_barrier",
                "영혼방벽",
                "다음에 받는 피해를 5 감소시킨다.",
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
                { poison.Id, poison },
                { wet.Id, wet },
                { frozen.Id, frozen },
                { bind.Id, bind },
                { magicCharge.Id, magicCharge },
                { magicOverload.Id, magicOverload },
                { magicStoneEater.Id, magicStoneEater },
                { fixedEffect.Id, fixedEffect },
                { packTactics.Id, packTactics },
                { spiritBarrier.Id, spiritBarrier }
            };
        }
    }
}
