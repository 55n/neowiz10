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
                    new RoomSlotType(RoomObjectType.Trap, "arrow_trap", false),
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
                    new RoomSlotType(RoomObjectType.Trap, "arrow_trap", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "room3_treasure_chest", false),
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
                    { RoomDirection.LEFT, new RoomEdgeType(null, false) },
                    { RoomDirection.RIGHT, new RoomEdgeType(null, false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Pile, "room4_loser_mark_pile", false),
                });
            RoomType room5 = new RoomType(
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
                    { RoomDirection.LEFT, new RoomEdgeType(null, false) },
                    { RoomDirection.RIGHT, new RoomEdgeType(null, false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty", false),
                    new RoomSlotType(RoomObjectType.Pile, "pile", false),
                });
            
            RoomType room28 = new RoomType(
                "room-28",
                "출구",
                "끝!",
                new List<string>    
                {
                    "지상으로 올라가는 사다리다",
                    "당신은 기쁨의 눈물을 흘렸다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.BACK, new RoomEdgeType(null, false) },
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
                { room28.Id, room28 }
            };
        }
    }
}
