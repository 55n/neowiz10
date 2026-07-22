using System;
using System.Collections.Generic;

namespace Darkness
{
    public class RoomMoveInterceptorFactory
    {
        private readonly Dictionary<
            string,
            Func<RoomSlotContentFactory, IMoveInterceptor>>
            factories;

        public RoomMoveInterceptorFactory()
        {
            factories = new Dictionary<
                string,
                Func<RoomSlotContentFactory, IMoveInterceptor>>();
            Register(
                "room-11",
                contentFactory =>
                    new Room11MoveInterceptor(contentFactory));
        }

        public void Register(
            string roomId,
            Func<RoomSlotContentFactory, IMoveInterceptor> factory)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                throw new ArgumentException(
                    "A room id is required.",
                    "roomId");
            }

            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            factories[roomId] = factory;
        }

        public IMoveInterceptor Create(
            string roomId,
            RoomSlotContentFactory contentFactory)
        {
            Func<RoomSlotContentFactory, IMoveInterceptor> factory;
            return !string.IsNullOrEmpty(roomId) &&
                   factories.TryGetValue(roomId, out factory)
                ? factory(contentFactory)
                : null;
        }
    }
}
