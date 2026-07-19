using System.Collections.Generic;

namespace Darkness
{
    public class SlotInteractionResult
    {
        public List<string> Messages { get; private set; }
        public List<AttackContext> Attacks { get; private set; }
        public List<DamageContext> Damages { get; private set; }
        public List<MonsterMoveRequest> MonsterMoves { get; private set; }
        public bool RevealSlot { get; set; }
        public bool RemoveContent { get; set; }

        public SlotInteractionResult()
        {
            Messages = new List<string>();
            Attacks = new List<AttackContext>();
            Damages = new List<DamageContext>();
            MonsterMoves = new List<MonsterMoveRequest>();
        }

        public void Merge(SlotInteractionResult other)
        {
            if (other == null)
            {
                return;
            }

            Messages.AddRange(other.Messages);
            Attacks.AddRange(other.Attacks);
            Damages.AddRange(other.Damages);
            MonsterMoves.AddRange(other.MonsterMoves);
            RevealSlot = RevealSlot || other.RevealSlot;
            RemoveContent = RemoveContent || other.RemoveContent;
        }
    }
}
