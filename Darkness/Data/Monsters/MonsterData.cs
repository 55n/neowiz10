using System.Collections.Generic;
using System.Linq;

namespace Darkness
{
    public class MonsterData
    {
        public Dictionary<string, MonsterType> MonsterTypes { get; private set; }

        public MonsterData()
        {
            MonsterTypes = new List<MonsterType>
            {
                Monster("fall", "낙하", "끝을 알 수 없는 높이에서 바닥으로 추락한다.",
                    1, 0, 999999, 0, 0, 999999, 0,
                    canBePoisoned: false),
                Monster("lost_goblin", "고블린", "어둠에 적응하지 못한 작고 겁 많은 잡식성 생물이다.",
                    5, 2, 2, 0, 4, 70, 20,
                    Effects(), MonsterFunction.Damage, Skills("goblin_rush")),
                Monster("hungry_troll", "굶은트롤", "후각으로 먹이를 추적하며 상처가 빠르게 아물어 가는 대형 포식자다.",
                    24, 3, 6, 3, 1, 75, 5,
                    Effects(), MonsterFunction.Damage, Skills("troll_regeneration")),
                Monster("bound_armor_spirit", "갑주령", "갑옷과 방에 속박되어 마석으로 기억과 영혼을 유지하는 옛 탐험가다.",
                    30, 5, 7, 6, 2, 85, 10,
                    Effects(Target("bind", 40), Target("bind", 40)), MonsterFunction.Damage, Skills("armor_chain_bind"),
                    false),
                Monster("coffin_undead", "관 속의 망자", "오랜 잠에서 깨어나 관을 밀치고 일어난 언데드다.",
                    12, 0, 4, 2, 1, 75, 5,
                    canBePoisoned: false),
                Monster("frost_wolf", "서리늑대", "후각과 무리 신호가 발달한 미궁의 육식동물이다.",
                    10, 3, 4, 1, 5, 80, 30,
                    Effects(), MonsterFunction.Damage, Skills("wolf_pack_assault")),
                Monster("echo_bat", "박쥐", "반향 기관으로 완전한 어둠 속을 비행하는 소형 포식자다.",
                    5, 2, 2, 0, 6, 75, 40,
                    Effects(Target("confusion", 30), Target("confusion", 30), Target("confusion", 30)),
                    MonsterFunction.DrainFocus, Skills("bat_echo_strike")),
                Monster("vibration_worm", "진동벌레", "바닥 아래에 숨어 충격과 체중 이동을 기다리는 매복 생물이다.",
                    14, 2, 5, 4, 2, 80, 10,
                    Effects(Target("fracture", 30), Target("fracture", 30), Target("fracture", 30)),
                    MonsterFunction.Damage, Skills("worm_ambush")),
                Monster("swamp_specter", "늪귀신", "물길과 젖은 바닥을 영역으로 삼는 뱀처럼 생긴 습윤 포식자다.",
                    12, 4, 4, 2, 3, 75, 20,
                    Effects(Target("fear", 35), Target("fear", 35), Target("fear", 35), Target("fear", 35)),
                    MonsterFunction.DrainFocus, Skills("water_wail")),
                Monster("echo_mimic", "메아리꾼", "포식한 생물의 소리를 복제해 가짜 위치로 먹이를 유인하는 도마뱀을 닮은 포식자다.",
                    8, 4, 3, 1, 4, 70, 25,
                    Effects(Target("confusion", 50), Target("confusion", 50), Target("confusion", 50), Target("confusion", 50), Target("confusion", 50)),
                    MonsterFunction.DrainFocus, Skills("echo_decoy")),
                Monster("breath_hunter", "숨사냥꾼", "살아 있는 먹이의 호흡과 체취를 끝까지 좇는 미궁의 최상위 포식자다.",
                    22, 5, 7, 3, 6, 90, 35,
                    Effects(Target("tracking", 100), Target("tracking", 100), Target("tracking", 100)),
                    MonsterFunction.Damage, Skills("breath_hunt")),
                Monster("door_clinger", "문붙이", "문과 통로에 붙어 지나가는 생물을 기다리는 부착형 포식자다.",
                    18, 3, 5, 5, 1, 85, 5,
                    Effects(Target("bind", 50), Target("bind", 50)), MonsterFunction.Damage, Skills("door_bind"))
            }.ToDictionary(monsterType => monsterType.Id);
        }

        private static MonsterType Monster(
            string id,
            string name,
            string description,
            int maxHealth,
            int maxFocus,
            int attack,
            int defense,
            int speed,
            int accuracy,
            int evasion,
            List<EffectApplication> attackEffects = null,
            MonsterFunction attackFunction = MonsterFunction.Damage,
            List<string> focusSkillIds = null,
            bool canBePoisoned = true)
        {
            return new MonsterType(
                id, name, description, maxHealth, maxFocus, attack, defense,
                speed, accuracy, evasion,
                attackEffects ?? Effects(), attackFunction,
                focusSkillIds ?? Skills(), canBePoisoned);
        }

        private static List<string> Skills(params string[] skillIds)
        {
            return new List<string>(skillIds);
        }

        private static List<EffectApplication> Effects(
            params EffectApplication[] effects)
        {
            return new List<EffectApplication>(effects);
        }

        private static EffectApplication Target(string effectId, int applyChance)
        {
            return new EffectApplication(
                effectId,
                EffectTarget.Target,
                applyChance);
        }
    }
}
