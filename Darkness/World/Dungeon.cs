using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public class Dungeon
    {
        public const string StartingRoomId = "room-0";

        public Dictionary<string, Room> Rooms { get; private set; }
        public Room CurrentRoom { get; private set; }

        public Dungeon()
            : this(new RoomMoveInterceptorFactory())
        {
        }

        public Dungeon(
            RoomMoveInterceptorFactory moveInterceptorFactory)
        {
            if (moveInterceptorFactory == null)
            {
                throw new ArgumentNullException(
                    "moveInterceptorFactory");
            }

            RoomData roomData = new RoomData();
            RoomSlotContentFactory contentFactory =
                new RoomSlotContentFactory();
            RoomTurnBehaviorFactory turnBehaviorFactory =
                new RoomTurnBehaviorFactory();

            Rooms = new Dictionary<string, Room>();

            foreach (KeyValuePair<string, RoomType> pair in roomData.RoomTypes)
            {
                Rooms.Add(
                    pair.Key,
                    new Room(
                        pair.Value,
                        contentFactory,
                        moveInterceptorFactory.Create(
                            pair.Key,
                            contentFactory),
                        turnBehaviorFactory.Create(
                            pair.Key,
                            contentFactory)));
            }

            foreach (KeyValuePair<string, RoomType> roomPair in roomData.RoomTypes)
            {
                Room room = Rooms[roomPair.Key];

                foreach (KeyValuePair<RoomDirection, RoomEdgeType> edgePair
                    in roomPair.Value.Edges)
                {
                    RoomEdgeType edgeType = edgePair.Value;

                    Room targetRoom;
                    if (edgeType.TargetRoomId == null ||
                        !Rooms.TryGetValue(
                            edgeType.TargetRoomId,
                            out targetRoom))
                    {
                        continue;
                    }

                    room.Edges.Add(
                        edgePair.Key,
                        new RoomEdge(targetRoom, edgeType.InitiallyLocked));
                }
            }

            CurrentRoom = Rooms[StartingRoomId];
        }

        public void Move(RoomDirection direction)
        {
            CurrentRoom = CurrentRoom.Edges[direction].TargetRoom;
        }

        public bool MoveTo(string roomId)
        {
            Room room;
            if (string.IsNullOrEmpty(roomId) ||
                !Rooms.TryGetValue(roomId, out room))
            {
                return false;
            }

            CurrentRoom = room;
            return true;
        }
    }
}
