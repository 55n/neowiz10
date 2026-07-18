using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    public class Dungeon
    {
        public Dictionary<string, Room> Rooms { get; private set; }
        public Room CurrentRoom { get; private set; }

        public Dungeon()
        {
            RoomData roomData = new RoomData();

            Rooms = new Dictionary<string, Room>();

            foreach (KeyValuePair<string, RoomType> pair in roomData.RoomTypes)
            {
                Rooms.Add(pair.Key, new Room(pair.Value));
            }

            foreach (KeyValuePair<string, RoomType> roomPair in roomData.RoomTypes)
            {
                Room room = Rooms[roomPair.Key];

                foreach (KeyValuePair<RoomDirection, RoomEdgeType> edgePair
                    in roomPair.Value.Edges)
                {
                    RoomEdgeType edgeType = edgePair.Value;

                    if (edgeType.TargetRoomId == null)
                    {
                        continue;
                    }

                    Room targetRoom = Rooms[edgeType.TargetRoomId];

                    room.Edges.Add(
                        edgePair.Key,
                        new RoomEdge(targetRoom, edgeType.InitiallyLocked));
                }
            }

            CurrentRoom = Rooms["room-0"];
        }
    }
}
