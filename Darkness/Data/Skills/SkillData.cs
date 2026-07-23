using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class SkillData
    {
        public Dictionary<string, SkillType> SkillTypes { get; private set; }

        public SkillData()
        {
            SkillTypes = new List<SkillType>
            {
                new SkillType("cowardly_leap", "비겁한 도약", "마석을 소모해 출구로 도망친다.", SkillCostType.MagicStone, 4, false, Effects()),
                new SkillType("developer_strike", "개발자의 일격", "모든 슬롯에 방어 무시 고정 피해 10000을 준다.", SkillCostType.Focus, 0, false, SkillTargetingType.None, Effects(EffectApplication.Damage(EffectTarget.AllDamageableRoomContents, 10000, 100, true))),
                new SkillType("last_focus", "마지막 집중", "다음 공격의 정확도에 100을 더한다.", SkillCostType.Focus, 5, false, SkillTargetingType.None, Effects(EffectApplication.ApplyStatus("perfect_focus", EffectTarget.Self))),
                new SkillType("power_strike", "강타", "집중을 소모해 공격력의 150% 피해로 공격한다.", SkillCostType.Focus, 1, false, SkillTargetingType.SingleSlot, SkillAttackType.Weapon, Effects(EffectApplication.Attack(EffectTarget.Target, 150, 100, AttackDeliveryType.EquippedWeapon))),
                new SkillType("sweep", "휩쓸기", "창을 크게 휘둘러 모든 슬롯의 공격 가능한 대상을 공격한다.", SkillCostType.Focus, 5, false, SkillTargetingType.None, SkillAttackType.Weapon, Effects(EffectApplication.Attack(EffectTarget.AllRoomSlots, 100, 100, AttackDeliveryType.EquippedWeapon))),
                new SkillType("guardian_blessing", "수호의 가호", "자신에게 수호의 가호를 부여해 치명적인 피해를 한 번 막는다.", SkillCostType.MagicStone, 5, false, SkillTargetingType.None, Effects(EffectApplication.ApplyStatus("guardian_blessing", EffectTarget.Self)), "{0} 은(는) 한 번의 죽음을 면할 수 있게 되었다"),
                new SkillType("sword_last_day", "칼의 최후", "장착한 무기의 공격력을 2배로 만들고 내구도를 1로 만든다.", SkillCostType.MagicStone, 2, false, SkillTargetingType.None, SkillAttackType.Weapon, Effects(EffectApplication.ApplyStatus("weapon_empowerment", EffectTarget.EquippedWeapon), EffectApplication.SetEquipmentDurability("weapon_consumption", EffectTarget.Self, EquipmentSlot.Weapon, 1))),
                new SkillType("winter_wind", "겨울바람", "차가운 바람을 일으켜 지정 슬롯의 콘텐츠에 [얼어붙음]을 부여한다.", SkillCostType.Focus, 1, false, SkillTargetingType.SingleSlot, Effects(EffectApplication.ApplyStatus("frozen", EffectTarget.Target)), "차가운 바람이 {0}을(를) 휘감았다. {0}이(가) 얼어붙었다."),
                new SkillType("water_splash", "물보라", "집중을 소모해 지정 슬롯의 대상에게 [젖음]을 부여한다.", SkillCostType.Focus, 2, false, SkillTargetingType.SingleSlot, Effects(EffectApplication.ApplyStatus("wet", EffectTarget.Target)), "물보라가 {0}을(를) 휘감았다. {0}이(가) 흠뻑 젖었다."),
                new SkillType("chain_bind", "사슬결박", "집중 3을 소모해 지정 슬롯의 대상에게 속박 2스택을 부여한다.", SkillCostType.Focus, 3, false, SkillTargetingType.SingleSlot, Effects(EffectApplication.ApplyStatus("bind", EffectTarget.Target, 100, 2))),
                new SkillType("spirit_barrier", "영혼방벽", "집중을 소모해 다음에 받는 피해를 5 감소시킨다.", SkillCostType.Focus, 2, false, Effects(EffectApplication.ApplyStatus("spirit_barrier", EffectTarget.Self))),
                new SkillType("magic_recollection", "마력회상", "마석 1개를 소모해 집중력을 3 회복한다.", SkillCostType.MagicStone, 1, false, Effects(EffectApplication.RestoreFocus(EffectTarget.Self, 3)), "흐릿했던 기억이 되살아나며 {0}의 집중력이 회복되었다."),

                new SkillType("goblin_rush", "겁먹은 돌진", "명중률이 50 하락하고 5의 피해를 주는 돌진 공격을 한다", SkillCostType.Focus, 1, false, SkillTargetingType.SingleSlot, SkillAttackType.Natural, Effects(EffectApplication.FixedAttack(EffectTarget.Target, 5, -50))),
                new SkillType("waterside_ambush", "물가 기습", "물속에서 튀어나와 8의 피해를 주고 다른 물길로 이동한다.", SkillCostType.Focus, 4, false, SkillTargetingType.SingleSlot, SkillAttackType.Natural, Effects(EffectApplication.Damage(EffectTarget.Target, 8, 100, true)), NarrativeTokens.Actor + "이 발밑의 물속에서 튀어나와 {0}을(를) 덮친 뒤 다른 물길 속으로 사라졌다.", SkillFollowUpType.Move),
                new SkillType("web_bind", "거미줄 낙하", "천장의 거미줄이 떨어져 대상에게 속박 1스택을 부여한다.", SkillCostType.Focus, 0, false, SkillTargetingType.SingleSlot, Effects(Target("bind")))
            }.ToDictionary(skillType => skillType.Id);
        }

        private static List<EffectApplication> Effects(
            params EffectApplication[] effects)
        {
            return new List<EffectApplication>(effects);
        }

        private static EffectApplication Target(string effectId)
        {
            return new EffectApplication(effectId, EffectTarget.Target, 100);
        }
    }
}
