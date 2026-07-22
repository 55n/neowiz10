namespace Darkness
{
    public class PoisonEffect : ActiveEffect
    {
        private const int TurnEndDamage = 3;

        public PoisonEffect(EffectType type, object source)
            : base(type, source)
        {
        }

        public override bool CanApplyTo(IEffectTarget target)
        {
            IPoisonable poisonable = target as IPoisonable;
            return poisonable != null && poisonable.CanBePoisoned;
        }

        public override void OnTurnEnd(EffectTurnContext context)
        {
            if (!IsActiveInRoom(context.Room))
            {
                context.EffectsToRemove.Add(this);
                return;
            }

            context.Damages.Add(new DamageContext(
                this,
                context.Target,
                TurnEndDamage,
                true));
        }

        public override bool IsActiveInRoom(Room room)
        {
            IRoomEffectSource source = Source as IRoomEffectSource;
            if (room == null || source == null ||
                !source.IsRoomEffectActive)
            {
                return false;
            }

            return room.Slots.Exists(slot =>
                ReferenceEquals(slot.Content, source.EffectSource));
        }
    }
}
