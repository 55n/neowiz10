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
                    1, 0, 999999, 0, 999999, 0,
                    canBePoisoned: false),
                Monster("lost_goblin", "고블린", "어둠에 적응하지 못한 작고 겁 많은 잡식성 생물이다.",
                    5, 2, 2, 0, 70, 20,
                    Skills("goblin_rush")),
                Monster("hungry_troll", "굶은트롤", "후각으로 먹이를 추적하며 상처가 빠르게 아물어 가는 대형 포식자다.",
                    22, 3, 6, 2, 75, 5),
                Monster("bound_armor_spirit", "갑주령", "갑옷과 방에 속박되어 마석으로 기억과 영혼을 유지하는 옛 탐험가다.",
                    36, 5, 8, 6, 85, 15,
                    canBePoisoned: false),
                Monster("coffin_undead", "관속망자", "오랜 잠에서 깨어나 관을 밀치고 일어난 언데드다.",
                    12, 0, 4, 2, 75, 5,
                    canBePoisoned: false),
                Monster("frost_wolf", "서리늑대", "후각과 무리 신호가 발달한 미궁의 육식동물이다.",
                    12, 3, 4, 1, 80, 25),
                Monster("echo_bat", "박쥐", "반향 기관으로 완전한 어둠 속을 비행하는 소형 포식자다.",
                    5, 2, 2, 0, 75, 40),
                Monster("room14_vibration_worm", "진동벌레", "모래 아래에 잠복해 지면의 진동을 추적하는 거대한 포식자다.",
                    100, 10, 100, 20, 120, 0,
                    canBePoisoned: false),
                Monster("swamp_specter", "늪귀신", "물길과 젖은 바닥을 영역으로 삼는 뱀처럼 생긴 습윤 포식자다.",
                    14, 4, 4, 2, 80, 15,
                    Skills("waterside_ambush")),
                Monster("echo_mimic", "메아리꾼", "다른 생물의 소리를 흉내 내 가짜 위치로 먹이를 유인하는 포식자다.",
                    9999, 4, 3, 1, 70, 25),
                Monster("breath_hunter_spiderling", "숨추적자", "갓 부화해 아직 껍질도 굳지 않은 작은 거미다.",
                    1, 0, 2, 0, 70, -100),
                Monster("ceiling_web", "거미줄", "천장에서 먹잇감이 지나가기를 기다리는 팽팽한 거미줄이다.",
                    1, 0, 0, 0, 100, -100,
                    Skills("web_bind"),
                    false),
                Monster("breath_hunter_part_left_foreleg", "좌앞다리", "바닥의 움직임을 더듬고 먹잇감을 후려치는 기다란 앞다리다.",
                    16, 0, 4, 2, 85, 10),
                Monster("breath_hunter_part_head", "송곳머리", "한 번에 먹잇감을 꿰뚫는 거대한 송곳니가 달린 머리다.",
                    20, 0, 9, 3, 90, 5),
                Monster("breath_hunter_part_abdomen", "복부", "다섯 부위를 지탱하는 크고 단단한 중심부다.",
                    32, 0, 0, 5, 0, 0),
                Monster("breath_hunter_part_spinneret", "방적기관", "끈적한 실을 뿜어 먹잇감의 움직임을 봉쇄하는 기관이다.",
                    12, 0, 0, 1, 100, 0,
                    Skills("web_bind")),
                Monster("breath_hunter_part_right_foreleg", "우앞다리", "무거운 체중을 실어 먹잇감을 내려찍는 앞다리다.",
                    16, 0, 5, 2, 85, 10),
                Monster("door_clinger_tentacle", "촉수", "먹잇감을 휘감아 움직임을 봉쇄하는 길고 질긴 촉수다.",
                    10, 0, 3, 1, 85, 20),
                Monster("door_clinger_leg", "다리", "몸을 지탱하며 빠르게 위치를 바꾸는 관절진 다리다.",
                    12, 0, 4, 2, 80, 30),
                Monster("door_clinger_antenna", "더듬이", "미세한 진동과 마력의 흐름을 감지하는 가느다란 더듬이다.",
                    8, 0, 2, 0, 90, 25),
                Monster("door_clinger_torso", "몸통", "다섯 부위를 연결하고 마력을 순환시키는 단단한 중심부다.",
                    24, 0, 5, 5, 75, 5),
                Monster("door_clinger_claw", "갈퀴", "먹잇감의 살점을 뜯어내기 위해 발달한 무거운 갈퀴다.",
                    14, 0, 7, 2, 80, 10)
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
            int accuracy,
            int evasion,
            List<string> focusSkillIds = null,
            bool canBePoisoned = true)
        {
            return new MonsterType(
                id, name, description, maxHealth, maxFocus, attack, defense,
                accuracy, evasion,
                focusSkillIds ?? Skills(), canBePoisoned);
        }

        private static List<string> Skills(params string[] skillIds)
        {
            return new List<string>(skillIds);
        }

    }
}
