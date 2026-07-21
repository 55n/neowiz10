using System.Collections.Generic;

namespace Darkness
{
    public interface IRoomEffectSource
    {
        object EffectSource { get; }
        EffectOriginType EffectOriginType { get; }
        string EffectOriginId { get; }
        bool IsRoomEffectActive { get; }
        IEnumerable<EffectApplication> RoomEffects { get; }
    }
}
