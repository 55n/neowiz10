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
                "",
                new List<string>
                {
                    "어두운 공간에는 적막 만이 흐르고 있다",
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType("room-1", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                });

            RoomType room1 = new RoomType(
                "room-1",
                "첫번째 방",
                "",
                new List<string>    
                {
                    "기척이 느껴진다",
                    "무언가가 긁는 소리를 내고 있다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType("room-2", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Monster, "goblin-0", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                });

            RoomType room2 = new RoomType(
                "room-2",
                "두번째 방",
                "",
                new List<string>    
                {
                    "조용하다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType("room-3", false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", true),
                    new RoomSlotType(RoomObjectType.Trap, "arrow-trap-0", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "treasure-chest-0", false),
                });

            RoomType room3 = new RoomType(
                "room-3",
                "세번째 방",
                "",
                new List<string>    
                {
                    "거대한 무언가가 킁킁거리고 있다",
                    "짐승 같은 악취가 코를 찌른다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.FORWARD, new RoomEdgeType(null, false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Monster, "troll-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", true),
                    new RoomSlotType(RoomObjectType.Trap, "arrow-trap-0", false),
                    new RoomSlotType(RoomObjectType.TreasureChest, "treasure-chest-0", false),
                });
            RoomType room4 = new RoomType(
                "room-4",
                "휴식공간",
                "",
                new List<string>    
                {
                    "천장에 붙은 벌레들에서 희미한 빛이 나오고 있다",
                    "당신은 안전한 것을 확인하고 잠시 쉬어가기로 했다"
                },
                new Dictionary<RoomDirection, RoomEdgeType>
                {
                    { RoomDirection.LEFT, new RoomEdgeType(null, false) },
                    { RoomDirection.RIGHT, new RoomEdgeType(null, false) }
                },
                new List<RoomSlotType>
                {
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", true),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Empty, "empty-0", false),
                    new RoomSlotType(RoomObjectType.Pile, "Pile-0", false),
                });

            RoomTypes = new Dictionary<string, RoomType>
            {
                { room0.Id, room0 }
            };
        }
    }
}
