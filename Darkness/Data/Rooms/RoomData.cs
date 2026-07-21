using System.Collections.Generic;

namespace Darkness
{
    public class RoomData
    {
        public Dictionary<string, RoomType> RoomTypes { get; private set; }

        public RoomData()
        {
            RoomType room0 = new RoomType(
                "room-0",
                "추락지점",
                "암흑이 눈앞에 펼쳐져 있다. 손을 뻗어 확인하는 수밖에 없을 것 같다.",
                new List<string>
                {
                    "어두운 공간에는 적막 만이 흐르고 있다",
                    "당신은 공간을 탐색하기로 했다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType("room-1", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });

            RoomType room1 = new RoomType(
                "room-1",
                "첫번째 방",
                "어둠 속에서 뭐가 튀어나올지 모른다",
                new List<string>    
                {
                    "기척이 느껴진다",
                    "조그만 무언가가 긁는 소리를 내고 있다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-0", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-2", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Monster, "room1_lost_goblin", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });

            RoomType room2 = new RoomType(
                "room-2",
                "두번째 방",
                "꽤 조용하다",
                new List<string>    
                {
                    "적막이 맴돈다",
                    "아주 작은 금속음이 간신히 들린다",
                    "불길한 예감이 든다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-1", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-3", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", true),
                    new RoomSlotType(RoomObjectType.Trap, "room2_arrow_trap", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "room2_treasure_chest", false),
                });

            RoomType room3 = new RoomType(
                "room-3",
                "세번째 방",
                "악취가 진동을 한다",
                new List<string>    
                {
                    "짐승 같은 악취가 코를 찌른다",
                    "거대한 무언가가 킁킁거리고 있다",
                    "배가 고픈 것 같다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-2", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-4", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Monster, "room3_hungry_troll", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });
            RoomType room4 = new RoomType(
                "room-4",
                "휴식공간",
                "빛이 있다는 사실에 안도감을 느낀다",
                new List<string>    
                {
                    "천장에 붙은 벌레들에서 희미한 빛이 나오고 있다",
                    "당신은 안전한 것을 확인하고 잠시 쉬어가기로 했다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-3", false) },
                    { RoomDirection.LEFT, new RoomEdgeType("room-7", false) },
                    { RoomDirection.RIGHT, new RoomEdgeType("room-5", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Pile, "room4_loser_mark_pile", false),
                },
                true);
            RoomType room5 = new RoomType(
                "room-5",
                "창고",
                "퀴퀴한 냄새가 난다",
                new List<string>    
                {
                    "여러 개의 기척이 느껴진다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-4", false) },
                    { RoomDirection.RIGHT, new RoomEdgeType("room-6", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Pile, "room5_supply_pile", true),
                    new RoomSlotType(RoomObjectType.Monster, "room5_goblin_1", false),
                    new RoomSlotType(RoomObjectType.Monster, "room5_goblin_2", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });
            RoomType room6 = new RoomType(
                "room-6",
                "무기고",
                "기름 찌든 내가 난다",
                new List<string>
                {
                    "누군가가 당신을 부른다",
                    "\"사람? 사람인가! 여기! 여기로 오게!\"",
                    "\"자네가 보고 있는 방향 그대로 가운데로 오면 되네!\"",
                    "\"해칠 생각은 없으니 걱정 말게나. 그냥 반가워서 그러네!\"",
                    "\"다만 미리 경고하는데 다른 쪽은 건들지 말게나. 그것들은 내 물건들이네.\"",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-5", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.TreasureChest, "room6_treasure_chest_1", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "room6_treasure_chest_2", false),
                    new RoomSlotType(RoomObjectType.Monster, "room6_armor_spirit", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "room6_treasure_chest_3", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "room6_treasure_chest_4", false),
                });
            RoomType room7 = new RoomType(
                "room-7",
                "돌무덤",
                "돌부스러기가 밟힌다",
                new List<string>
                {
                    "무언가로 앞이 가로막혀있다",
                    "망자의 말소리가 들려온다",
                    "\"쓸데 없이 열면 죽인다. 네 명은 진실을, 하나는 거짓을 말한다.\"",
                    "\"거짓말쟁이를 찾아라. 그자가 길을 막고 있다.\""
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-4", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-8", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Coffin, "room7_coffin_1", false),
                    new RoomSlotType(RoomObjectType.Coffin, "room7_coffin_2", false),
                    new RoomSlotType(RoomObjectType.Coffin, "room7_coffin_3", false),
                    new RoomSlotType(RoomObjectType.Coffin, "room7_coffin_4", false),
                    new RoomSlotType(RoomObjectType.Coffin, "room7_coffin_5", true),
                });
            RoomType room8 = new RoomType(
                "room-8",
                "맹독안개",
                "희미한 죽음의 향기",
                new List<string>
                {
                    "희미하게 매캐한 향이 공기에 남아있다",
                    "당신은 불길함을 애써 무시하고 손을 뻗고 천천히 전진한다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-7", false) },
                    { RoomDirection.RIGHT, new RoomEdgeType("room-9", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Body, "room8_trapped_skeleton", false),
                    new RoomSlotType(RoomObjectType.Body, "room8_antidote_skeleton", true),
                    new RoomSlotType(RoomObjectType.Body, "room8_skeleton_3", false),
                    new RoomSlotType(RoomObjectType.Body, "room8_skeleton_4", false),
                    new RoomSlotType(RoomObjectType.Body, "room8_skeleton_5", false),
                });
            RoomType room9 = new RoomType(
                "room-9",
                "박쥐둥지",
                "질척한 배설물이 밟힌다",
                new List<string>
                {
                    "숨돌릴 틈도 없이 박쥐들이 공격해왔다!",
                    "숫자가 어마어마하게 많은 것 같다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-8", false) },
                    { RoomDirection.LEFT, new RoomEdgeType("room-10", false) },
                    { RoomDirection.RIGHT, new RoomEdgeType("room-11", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Monster, "room9_echo_bat_1", true),
                    new RoomSlotType(RoomObjectType.Monster, "room9_echo_bat_2", false),
                    new RoomSlotType(RoomObjectType.Monster, "room9_echo_bat_3", false),
                    new RoomSlotType(RoomObjectType.Monster, "room9_echo_bat_4", false),
                    new RoomSlotType(RoomObjectType.Monster, "room9_echo_bat_5", false),
                },
                false,
                true);

            RoomType room10 = new RoomType(
                "room-10",
                "지하호수",
                "바닥에 물이 고여있다",
                new List<string>
                {
                    "걸음마다 찰박거리는 소리가 들린다",
                    "깊지는 않지만 다리가 젖는 것을 막을 수는 없었다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-9", false) },
                    { RoomDirection.RIGHT, new RoomEdgeType("room-12", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Monster, "room10_swamp_specter", false),
                    new RoomSlotType(RoomObjectType.Water, "room10_water_1", true),
                    new RoomSlotType(RoomObjectType.Water, "room10_water_2", false),
                    new RoomSlotType(RoomObjectType.Water, "room10_water_3", false),
                    new RoomSlotType(RoomObjectType.Water, "room10_water_4", false),
                });

            RoomType room11 = new RoomType(
                "room-11",
                "폐광",
                "매우 넓은 공동인 것 같다",
                new List<string>
                {
                    "정적이 맴돈다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-9", false) },
                    { RoomDirection.RIGHT, new RoomEdgeType("room-12", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Pile, "room11_magic_stone_pile", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });

            RoomType room12 = new RoomType(
                "room-12",
                "냉장고",
                "한기가 뼛속까지 스며든다",
                new List<string>
                {
                    "통로를 넘는 순간 공기가 급격히 차가워졌다",
                    "벽과 바닥을 훑는 손에 서리가 쓸려나간다",
                    "바닥에선 짐승의 냄새가 난다. 아마도 늑대...?",
                    "기다렸다는 듯이 어둠 속에서 늑대 울음소리가 들린다",
                    "갑작스러운 굉음이 뒤에서 난다", 
                    "당신은 본능적으로 앞으로 몸을 굴렀다",
                    "뒤를 돌아보니...아무래도 퇴로가 무너진 것 같다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType("room-13", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Monster, "room12_frost_wolf_1", false),
                    new RoomSlotType(RoomObjectType.Monster, "room12_frost_wolf_2", true),
                    new RoomSlotType(RoomObjectType.Monster, "room12_frost_wolf_3", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });
            RoomType room13 = new RoomType(
                "room-13",
                "메아리의 방",
                "소리가 울리는 방이다",
                new List<string>
                {
                    "멀리서 누군가가 소리를 지른다",
                    "\"여기야 여기!\"",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-12", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-14", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Echo, "room13_echo_1", false),
                    new RoomSlotType(RoomObjectType.Echo, "room13_echo_2", false),
                    new RoomSlotType(RoomObjectType.Echo, "room13_echo_3", false),
                    new RoomSlotType(RoomObjectType.Echo, "room13_echo_4", false),
                    new RoomSlotType(RoomObjectType.Echo, "room13_echo_5", true),
                });

            RoomType room14 = new RoomType(
                "room-14",
                "진동벌레둥지",
                "바닥이 부드러운 모래로 되어 있다",
                new List<string>
                {
                    "천장에서 모래가 폭포처럼 흘러 내리는 소리가 들린다",
                    "\"으악~! 살려줘~!!\"",
                    "당신만큼 불운한 누군가가 모래에 쓸려 어둠으로 떨어진 것 같다",
                    "푹 하고 모래에 파묻히는 소리" ,
                    "동시에 거대한 무언가가 모래에서 튀어나오며 비명을 집어삼킨다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-13", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-15", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Sand, "room14_sand_1", false),
                    new RoomSlotType(RoomObjectType.Sand, "room14_sand_2", false),
                    new RoomSlotType(RoomObjectType.Sand, "room14_sand_3", false),
                    new RoomSlotType(RoomObjectType.Sand, "room14_sand_4", false),
                    new RoomSlotType(RoomObjectType.Sand, "room14_sand_5", true),
                });
            RoomType room15 = new RoomType(
                "room-15",
                "숨추적자의 둥지",
                "매우 불길한 예감이 든다",
                new List<string>
                {
                    "무언가 불길한 예감이 당신을 덮친다" ,
                    "노련한 모험가로서의 본능이 경종을 울린다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType("room-14", false) },
                    { RoomDirection.FORWARD, new RoomEdgeType("room-16", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Prey, "room15_bound_troll", false),
                    new RoomSlotType(RoomObjectType.Egg, "room15_breath_hunter_egg_1", true),
                    new RoomSlotType(RoomObjectType.Prey, "room15_bound_goblin", false),
                    new RoomSlotType(RoomObjectType.Egg, "room15_breath_hunter_egg_2", false),
                    new RoomSlotType(RoomObjectType.Prey, "room15_bound_wolf", true),
                });
            RoomType room16 = new RoomType(
                "room-16",
                "사냥터",
                "이곳은 위험하다",
                new List<string>
                {
                    "무언가가 당신을 쫓고 있다",
                    "당신은 그것이 당신을 몰아 넣고 있다는 것을 깨달았다",
                    "당신은 절망에 빠졌다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType("room-17", false) },
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Trap, "room16_web_1", false),
                    new RoomSlotType(RoomObjectType.Trap, "room16_web_2", false),
                    new RoomSlotType(RoomObjectType.Trap, "room16_web_3", false),
                    new RoomSlotType(RoomObjectType.Trap, "room16_web_4", true),
                    new RoomSlotType(RoomObjectType.Trap, "room16_web_5", false),
                });

            RoomType room17 = new RoomType(
                "room-17",
                "출구",
                "끝!",
                new List<string>    
                {
                    "지상으로 올라가는 사다리다",
                    "당신은 기쁨의 눈물을 흘렸다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                });

            RoomTypes = new Dictionary<string, RoomType>
            {
                { room0.Id, room0 },
                { room1.Id, room1 },
                { room2.Id, room2 },
                { room3.Id, room3 },
                { room4.Id, room4 },
                { room5.Id, room5 },
                { room6.Id, room6 },
                { room7.Id, room7 },
                { room8.Id, room8 },
                { room9.Id, room9 },
            };
        }
    }
}
