using System;
using System.Collections.Generic;

namespace Darkness
{
    public class TurnManager
    {
        private readonly EffectType hastyEffectType;

        private class MonsterTurnEntry
        {
            public Monster Monster { get; private set; }
            public RoomSlot Slot { get; private set; }

            public MonsterTurnEntry(Monster monster, RoomSlot slot)
            {
                Monster = monster;
                Slot = slot;
            }
        }

        public int TurnNumber { get; private set; }
        public TurnPhase Phase { get; private set; }

        public TurnManager()
        {
            hastyEffectType = new EffectData().EffectTypes["hasty"];
            Phase = TurnPhase.PlayerInput;
        }

        public TurnResult Resolve(
            PlayerTurnCommand command,
            Hero hero,
            Room room)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (hero == null)
            {
                throw new ArgumentNullException("hero");
            }

            if (room == null)
            {
                throw new ArgumentNullException("room");
            }

            TurnResult turnResult = new TurnResult();
            if (!command.ConsumesTurn)
            {
                turnResult.TurnNumber = TurnNumber;
                return turnResult;
            }

            Phase = TurnPhase.PlayerAction;
            PlayerActionContext playerAction = new PlayerActionContext(
                hero,
                command.Action,
                command.TargetSlot,
                command.Item);
            ApplyPlayerAction(room, playerAction, turnResult);

            List<MonsterTurnEntry> monsters = SnapshotMonsters(room);

            Phase = TurnPhase.MonsterDecision;
            DecideMonsterActions(
                room,
                playerAction,
                monsters,
                turnResult);

            Phase = TurnPhase.MonsterAction;
            ExecuteMonsterActions(room, hero, monsters, turnResult);

            Phase = TurnPhase.EndTurn;
            TurnNumber++;
            turnResult.TurnCompleted = true;
            turnResult.TurnNumber = TurnNumber;
            turnResult.HeroDied = !hero.CanAct;
            Phase = TurnPhase.PlayerInput;
            return turnResult;
        }

        public TurnResult ResolveMove(
            Dungeon dungeon,
            RoomDirection direction,
            Hero hero)
        {
            if (dungeon == null)
            {
                throw new ArgumentNullException("dungeon");
            }

            if (hero == null)
            {
                throw new ArgumentNullException("hero");
            }

            Room room = dungeon.CurrentRoom;
            TurnResult turnResult = new TurnResult();

            Phase = TurnPhase.PlayerAction;
            hero.ApplyEffect(new HastyEffect(hastyEffectType));
            PlayerActionContext playerAction = new PlayerActionContext(
                hero,
                PlayerActionType.Move,
                null,
                null);
            List<MonsterTurnEntry> monsters = SnapshotMonsters(room);

            Phase = TurnPhase.MonsterDecision;
            DecideMonsterActions(
                room,
                playerAction,
                monsters,
                turnResult);

            Phase = TurnPhase.MonsterAction;
            ExecuteMonsterActions(room, hero, monsters, turnResult);
            hero.RemoveEffect(hastyEffectType.Id);

            Phase = TurnPhase.EndTurn;
            if (hero.CanAct)
            {
                dungeon.Move(direction);
                turnResult.RoomChanged = true;
            }

            TurnNumber++;
            turnResult.TurnCompleted = true;
            turnResult.TurnNumber = TurnNumber;
            turnResult.HeroDied = !hero.CanAct;
            Phase = TurnPhase.PlayerInput;
            return turnResult;
        }

        private void ApplyPlayerAction(
            Room room,
            PlayerActionContext context,
            TurnResult turnResult)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (context.Action == PlayerActionType.ThrowItem)
            {
                result.Merge(ItemThrowResolver.Resolve(context));
            }

            result.Merge(SlotInteractionResolver.Resolve(context));
            ApplyInteraction(
                room,
                context.TargetSlot,
                result,
                turnResult);
        }

        private List<MonsterTurnEntry> SnapshotMonsters(Room room)
        {
            List<MonsterTurnEntry> monsters =
                new List<MonsterTurnEntry>();
            foreach (RoomSlot slot in room.Slots)
            {
                Monster monster = slot.Content as Monster;
                if (monster != null && monster.CanAct)
                {
                    monsters.Add(new MonsterTurnEntry(monster, slot));
                }
            }

            return monsters;
        }

        private void DecideMonsterActions(
            Room room,
            PlayerActionContext playerAction,
            List<MonsterTurnEntry> monsters,
            TurnResult turnResult)
        {
            foreach (MonsterTurnEntry entry in monsters)
            {
                if (!entry.Monster.CanAct)
                {
                    continue;
                }

                MonsterDecision decision = entry.Monster.Decide(
                    new MonsterPerception(
                        playerAction,
                        room,
                        entry.Slot));
                if (!string.IsNullOrEmpty(decision.Message))
                {
                    turnResult.Messages.Add(decision.Message);
                }

                if (decision.RevealSlot)
                {
                    entry.Slot.Reveal();
                    AddChangedSlot(room, entry.Slot, turnResult);
                }
            }
        }

        private void ExecuteMonsterActions(
            Room room,
            Hero hero,
            List<MonsterTurnEntry> monsters,
            TurnResult turnResult)
        {
            foreach (MonsterTurnEntry entry in monsters)
            {
                if (!hero.CanAct)
                {
                    break;
                }

                if (!entry.Monster.CanAct)
                {
                    continue;
                }

                ApplyInteraction(
                    room,
                    entry.Slot,
                    entry.Monster.Act(hero),
                    turnResult);
            }
        }

        private void ApplyInteraction(
            Room room,
            RoomSlot slot,
            SlotInteractionResult result,
            TurnResult turnResult)
        {
            turnResult.Messages.AddRange(result.Messages);
            if (result.RemoveContent)
            {
                slot.ClearContent();
                AddChangedSlot(room, slot, turnResult);
            }

            foreach (AttackContext attack in result.Attacks)
            {
                AttackResolver.Resolve(attack);
            }

            foreach (DamageContext damage in result.Damages)
            {
                DamageResolver.Resolve(damage);
            }

            if (DropDefeatedMonsterLoot(slot))
            {
                AddChangedSlot(room, slot, turnResult);
            }

            foreach (MonsterMoveRequest move in result.MonsterMoves)
            {
                ApplyMonsterMove(room, move, turnResult);
            }

            if (result.RevealSlot)
            {
                slot.Reveal();
                AddChangedSlot(room, slot, turnResult);
            }
        }

        private bool DropDefeatedMonsterLoot(RoomSlot slot)
        {
            Monster monster = slot.Content as Monster;
            if (monster == null || monster.IsAlive)
            {
                return false;
            }

            monster.Inventory.TransferAllTo(slot.GroundInventory);
            slot.ClearContent();
            return true;
        }

        private void ApplyMonsterMove(
            Room room,
            MonsterMoveRequest request,
            TurnResult turnResult)
        {
            if (request == null || request.Monster == null ||
                !request.Monster.CanAct || request.TargetSlot == null ||
                !request.TargetSlot.IsEmpty)
            {
                return;
            }

            RoomSlot sourceSlot = room.Slots.Find(
                candidate => ReferenceEquals(
                    candidate.Content,
                    request.Monster));
            int sourceIndex = room.Slots.IndexOf(sourceSlot);
            int targetIndex = room.Slots.IndexOf(request.TargetSlot);
            if (sourceSlot == null || sourceSlot == request.TargetSlot ||
                sourceIndex < 0 || targetIndex < 0)
            {
                return;
            }

            sourceSlot.TakeContent();
            request.TargetSlot.SetContent(request.Monster);
            turnResult.ChangedSlotIndexes.Add(sourceIndex);
            turnResult.ChangedSlotIndexes.Add(targetIndex);
        }

        private void AddChangedSlot(
            Room room,
            RoomSlot slot,
            TurnResult turnResult)
        {
            int slotIndex = room.Slots.IndexOf(slot);
            if (slotIndex >= 0)
            {
                turnResult.ChangedSlotIndexes.Add(slotIndex);
            }
        }
    }
}
