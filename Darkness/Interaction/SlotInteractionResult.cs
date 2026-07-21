using System.Collections.Generic;

namespace Darkness
{
    public class SlotInteractionResult
    {
        public List<string> Messages { get; private set; }
        public List<AttackContext> Attacks { get; private set; }
        public List<DamageContext> Damages { get; private set; }
        public List<SkillUseContext> SkillUses { get; private set; }
        public List<ItemThrowPlan> ItemThrows { get; private set; }
        public List<EffectRequest> EffectRequests { get; private set; }
        public List<EquipmentDurabilityRequest> DurabilityRequests
        {
            get;
            private set;
        }
        public List<MonsterMoveRequest> MonsterMoves { get; private set; }
        public List<SlotContentChangeRequest> SlotContentChanges
        {
            get;
            private set;
        }
        public List<SlotStateChangeRequest> SlotStateChanges
        {
            get;
            private set;
        }
        public bool RemoveContent { get; set; }
        public bool LoudEventOccurred { get; set; }

        public SlotInteractionResult()
        {
            Messages = new List<string>();
            Attacks = new List<AttackContext>();
            Damages = new List<DamageContext>();
            SkillUses = new List<SkillUseContext>();
            ItemThrows = new List<ItemThrowPlan>();
            EffectRequests = new List<EffectRequest>();
            DurabilityRequests =
                new List<EquipmentDurabilityRequest>();
            MonsterMoves = new List<MonsterMoveRequest>();
            SlotContentChanges =
                new List<SlotContentChangeRequest>();
            SlotStateChanges =
                new List<SlotStateChangeRequest>();
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
            SkillUses.AddRange(other.SkillUses);
            ItemThrows.AddRange(other.ItemThrows);
            EffectRequests.AddRange(other.EffectRequests);
            DurabilityRequests.AddRange(other.DurabilityRequests);
            MonsterMoves.AddRange(other.MonsterMoves);
            SlotContentChanges.AddRange(other.SlotContentChanges);
            SlotStateChanges.AddRange(other.SlotStateChanges);
            RemoveContent = RemoveContent || other.RemoveContent;
            LoudEventOccurred =
                LoudEventOccurred || other.LoudEventOccurred;
        }
    }
}
