using System;
using System.Collections.Generic;

namespace Darkness
{
    public class TurnManager
    {
        private readonly EffectType hastyEffectType;
        private readonly EffectType defendingEffectType;
        private readonly SkillResolver skillResolver;
        private readonly DurabilityResolver durabilityResolver;
        private readonly EffectResolver effectResolver;

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
            EffectData effectData = new EffectData();
            hastyEffectType = effectData.EffectTypes["hasty"];
            defendingEffectType = effectData.EffectTypes["defending"];
            skillResolver = new SkillResolver();
            durabilityResolver = new DurabilityResolver();
            effectResolver = new EffectResolver();
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

            ValidateTarget(command, room);

            TurnResult turnResult = new TurnResult();
            if (!command.ConsumesTurn)
            {
                turnResult.TurnNumber = TurnNumber;
                return turnResult;
            }

            Phase = TurnPhase.PlayerAction;
            bool isDefending =
                command.Action == PlayerActionType.Defend;
            if (isDefending)
            {
                hero.ApplyEffect(
                    new DefendingEffect(
                        defendingEffectType));
                turnResult.Messages.Add(
                    CombatMessages.DefenseStanceTaken());
            }

            PlayerActionContext playerAction = new PlayerActionContext(
                hero,
                command.Action,
                command.TargetSlot,
                command.Item,
                command.SkillUse,
                command.ItemSourceEquipmentSlot);
            string playerActionMessage = HeroActionMessages.Started(
                command.Action,
                GetVisibleSlotTargetName(command.TargetSlot));
            if (!string.IsNullOrEmpty(playerActionMessage))
            {
                turnResult.Messages.Add(playerActionMessage);
            }

            bool loudEventOccurred = ApplyPlayerAction(
                room,
                playerAction,
                turnResult);

            List<MonsterTurnEntry> monsters = SnapshotMonsters(room);

            Phase = TurnPhase.MonsterDecision;
            DecideMonsterActions(
                room,
                playerAction,
                monsters,
                loudEventOccurred,
                turnResult);

            Phase = TurnPhase.MonsterAction;
            ExecuteMonsterActions(room, hero, monsters, turnResult);
            if (isDefending)
            {
                hero.RemoveEffect(defendingEffectType.Id);
            }

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
                null,
                null);
            List<MonsterTurnEntry> monsters = SnapshotMonsters(room);

            Phase = TurnPhase.MonsterDecision;
            DecideMonsterActions(
                room,
                playerAction,
                monsters,
                false,
                turnResult);

            Phase = TurnPhase.MonsterAction;
            ExecuteMonsterActions(room, hero, monsters, turnResult);
            hero.RemoveEffect(hastyEffectType.Id);

            Phase = TurnPhase.EndTurn;
            if (hero.CanAct)
            {
                dungeon.Move(direction);
                turnResult.RoomChanged = true;
                turnResult.Messages.Add(
                    HeroActionMessages.Moved(
                        dungeon.CurrentRoom.Type.Name));
            }

            TurnNumber++;
            turnResult.TurnCompleted = true;
            turnResult.TurnNumber = TurnNumber;
            turnResult.HeroDied = !hero.CanAct;
            Phase = TurnPhase.PlayerInput;
            return turnResult;
        }

        private bool ApplyPlayerAction(
            Room room,
            PlayerActionContext context,
            TurnResult turnResult)
        {
            SlotInteractionResult result = new SlotInteractionResult();
            if (context.Action == PlayerActionType.UseSkill &&
                context.SkillUse != null)
            {
                result.SkillUses.Add(context.SkillUse);
                ApplyInteraction(
                    room,
                    context.TargetSlot,
                    result,
                    turnResult);
                return result.LoudEventOccurred;
            }

            if (context.Action == PlayerActionType.ThrowItem)
            {
                result.Merge(ItemThrowResolver.Resolve(context));
            }

            if (PlayerActionRules.RequiresTargetSlot(context.Action))
            {
                result.Merge(SlotInteractionResolver.Resolve(context));
            }

            ApplyInteraction(
                room,
                context.TargetSlot,
                result,
                turnResult);
            return result.LoudEventOccurred;
        }

        private static void ValidateTarget(
            PlayerTurnCommand command,
            Room room)
        {
            if (command.Action == PlayerActionType.UseSkill)
            {
                ValidateSkillTarget(command, room);
                return;
            }

            if (PlayerActionRules.RequiresTargetSlot(command.Action) &&
                (command.TargetSlot == null ||
                 !room.Slots.Contains(command.TargetSlot)))
            {
                throw new ArgumentException(
                    "The action requires a target slot.",
                    "command");
            }
        }

        private static void ValidateSkillTarget(
            PlayerTurnCommand command,
            Room room)
        {
            if (command.SkillUse == null)
            {
                throw new ArgumentException(
                    "The skill action requires a skill context.",
                    "command");
            }

            SkillTargetingType targeting =
                command.SkillUse.Skill.TargetingType;
            if (targeting == SkillTargetingType.None)
            {
                if (command.TargetSlot != null)
                {
                    throw new ArgumentException(
                        "The skill does not use a target slot.",
                        "command");
                }

                return;
            }

            if (targeting == SkillTargetingType.SingleSlot)
            {
                if (command.TargetSlot == null ||
                    !room.Slots.Contains(command.TargetSlot))
                {
                    throw new ArgumentException(
                        "The skill requires a target slot.",
                        "command");
                }

                return;
            }

            throw new ArgumentException(
                "The skill targeting type is not supported.",
                "command");
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
            bool loudEventOccurred,
            TurnResult turnResult)
        {
            foreach (MonsterTurnEntry entry in monsters)
            {
                if (!entry.Monster.CanAct)
                {
                    continue;
                }

                MonsterState previousState = entry.Monster.State;
                MonsterDecision decision = entry.Monster.Decide(
                    new MonsterPerception(
                        playerAction,
                        room,
                        entry.Slot,
                        loudEventOccurred));
                string message = decision.Message;
                if (string.IsNullOrEmpty(message))
                {
                    message = MonsterActionMessages.DecisionFallback(
                        entry.Monster.Name,
                        previousState,
                        decision);
                }

                if (!string.IsNullOrEmpty(message))
                {
                    turnResult.Messages.Add(
                        GetVisibleDecisionMessage(
                            entry,
                            message));
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
                    entry.Monster.Act(hero, room),
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
                ISlotContent content = slot == null
                    ? null
                    : slot.Content;
                if (content != null)
                {
                    ApplySlotContentChange(
                        room,
                        SlotContentChangeRequest.Remove(
                            slot,
                            content),
                        turnResult);
                }
            }

            foreach (SkillUseContext skillUse in result.SkillUses)
            {
                SkillUseResult skillResult =
                    skillResolver.Resolve(skillUse);
                foreach (string message in skillResult.Messages)
                {
                    turnResult.Messages.Add(
                        GetVisibleSkillMessage(
                            room,
                            skillUse.User,
                            message));
                }
                result.Attacks.AddRange(skillResult.Attacks);
                result.Damages.AddRange(skillResult.Damages);
                result.DurabilityRequests.AddRange(
                    skillResult.DurabilityRequests);
            }

            foreach (ItemThrowPlan itemThrow in result.ItemThrows)
            {
                ResolveItemThrow(
                    room,
                    itemThrow,
                    result,
                    turnResult);
            }

            foreach (EquipmentDurabilityRequest request in
                     result.DurabilityRequests)
            {
                DurabilityResolveResult durabilityResult =
                    durabilityResolver.Resolve(request);
                turnResult.Messages.AddRange(
                    durabilityResult.Messages);
            }

            foreach (AttackContext attack in result.Attacks)
            {
                ResolveAttack(room, attack, turnResult);
            }

            foreach (DamageContext damage in result.Damages)
            {
                DamageResolver.Resolve(damage);
                turnResult.Messages.Add(
                    CombatMessages.DamageReceived(
                        GetVisibleName(room, damage.Target),
                        damage.FinalDamage));
            }

            Monster defeatedMonster = slot == null
                ? null
                : slot.Content as Monster;
            string defeatedMonsterName =
                defeatedMonster != null && !defeatedMonster.IsAlive
                    ? GetVisibleName(room, defeatedMonster)
                    : null;
            if (DropDefeatedMonsterLoot(
                    room,
                    slot,
                    defeatedMonster,
                    turnResult))
            {
                turnResult.Messages.Add(
                    CombatMessages.MonsterDefeated(
                        defeatedMonsterName));
            }

            TreasureChest destroyedChest = slot == null
                ? null
                : slot.Content as TreasureChest;
            string destroyedChestName =
                destroyedChest != null && destroyedChest.IsDestroyed
                    ? GetVisibleName(room, destroyedChest)
                    : null;
            if (RemoveDestroyedTreasureChest(
                    room,
                    slot,
                    destroyedChest,
                    turnResult))
            {
                turnResult.Messages.Add(
                    CombatMessages.ObjectDestroyed(
                        destroyedChestName));
            }

            ApplySlotContentChanges(
                room,
                result.SlotContentChanges,
                turnResult);

            foreach (MonsterMoveRequest move in result.MonsterMoves)
            {
                ApplyMonsterMove(room, move, turnResult);
            }

            ApplySlotStateChanges(
                room,
                result.SlotStateChanges,
                turnResult);

            AddDoorDiscoveryMessage(slot, turnResult);
        }

        private void ResolveItemThrow(
            Room room,
            ItemThrowPlan plan,
            SlotInteractionResult interactionResult,
            TurnResult turnResult)
        {
            if (plan == null || plan.ImpactAttack == null)
            {
                return;
            }

            AttackResult attackResult = ResolveAttack(
                room,
                plan.ImpactAttack,
                turnResult);
            if (!attackResult.IsHit || plan.Target.CurrentHealth <= 0 ||
                plan.OnHitEffects.Count == 0)
            {
                return;
            }

            EffectPlan effectPlan;
            if (!effectResolver.TryPrepare(
                    plan.OnHitEffects,
                    EffectContext.FromItemThrow(plan, room),
                    out effectPlan))
            {
                return;
            }

            EffectResolveResult effectResult =
                effectResolver.Execute(effectPlan);
            interactionResult.Attacks.AddRange(effectResult.Attacks);
            interactionResult.Damages.AddRange(effectResult.Damages);
            interactionResult.DurabilityRequests.AddRange(
                effectResult.DurabilityRequests);
        }

        private AttackResult ResolveAttack(
            Room room,
            AttackContext attack,
            TurnResult turnResult)
        {
            RevealAttackingSlot(room, attack, turnResult);
            turnResult.Messages.Add(
                CombatMessages.AttackStarted(
                    GetVisibleName(room, attack.Source),
                    GetVisibleName(room, attack.Target)));

            AttackResult attackResult = AttackResolver.Resolve(attack);
            Monster hitMonster = attack.Target as Monster;
            if (attackResult.IsHit && hitMonster != null)
            {
                hitMonster.RegisterHit();
            }

            DurabilityResolveResult durabilityResult =
                durabilityResolver.ResolveAttack(
                    attack,
                    attackResult,
                    room);
            if (attackResult.IsHit)
            {
                turnResult.Messages.Add(
                    CombatMessages.DamageReceived(
                        GetVisibleName(room, attack.Target),
                        attackResult.Damage));
            }
            else
            {
                turnResult.Messages.Add(
                    CombatMessages.DamageEvaded(
                        GetVisibleName(room, attack.Target)));
            }

            turnResult.Messages.AddRange(durabilityResult.Messages);
            return attackResult;
        }

        private void RevealAttackingSlot(
            Room room,
            AttackContext attack,
            TurnResult turnResult)
        {
            if (!(attack.Target is Hero))
            {
                return;
            }

            RoomSlot sourceSlot = FindContentSlot(
                room,
                attack.Source);
            if (sourceSlot == null ||
                sourceSlot.State == SlotState.REVEALED)
            {
                return;
            }

            ApplySlotStateChanges(
                room,
                new[]
                {
                    SlotStateChangeRequest.Reveal(sourceSlot)
                },
                turnResult);
        }

        private static string GetVisibleDecisionMessage(
            MonsterTurnEntry entry,
            string message)
        {
            if (entry.Slot.State == SlotState.REVEALED)
            {
                return message;
            }

            return message.Replace(entry.Monster.Name, "???");
        }

        private static string GetVisibleName(
            Room room,
            IDamageable target)
        {
            RoomSlot slot = FindContentSlot(room, target);
            return slot != null &&
                   slot.State != SlotState.REVEALED
                ? "???"
                : target.Name;
        }

        private static string GetVisibleSkillMessage(
            Room room,
            ISkillUser user,
            string message)
        {
            IDamageable source = user as IDamageable;
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }

            string visibleMessage = message;
            if (source != null)
            {
                string visibleName = GetVisibleName(room, source);
                if (visibleName != source.Name)
                {
                    visibleMessage = visibleMessage.Replace(
                        source.Name,
                        visibleName);
                }
            }

            foreach (RoomSlot slot in room.Slots)
            {
                if (slot.State != SlotState.REVEALED &&
                    slot.Content != null &&
                    !string.IsNullOrEmpty(slot.Content.Name))
                {
                    visibleMessage = visibleMessage.Replace(
                        slot.Content.Name,
                        "???");
                }
            }

            return visibleMessage;
        }

        private static string GetVisibleSlotTargetName(RoomSlot slot)
        {
            if (slot == null)
            {
                return "주변";
            }

            if (slot.State != SlotState.REVEALED)
            {
                return slot.Content == null
                    ? "어두운 공간"
                    : "???";
            }

            if (slot.Content != null)
            {
                return slot.Content.Name;
            }

            return slot.Type.HasDoor
                ? ExplorationMessages.Door()
                : "빈 공간";
        }

        private static RoomSlot FindContentSlot(
            Room room,
            object content)
        {
            return room == null
                ? null
                : room.Slots.Find(
                    slot => ReferenceEquals(
                        slot.Content,
                        content));
        }

        private bool DropDefeatedMonsterLoot(
            Room room,
            RoomSlot slot,
            Monster monster,
            TurnResult turnResult)
        {
            if (slot == null || monster == null || monster.IsAlive ||
                !ReferenceEquals(slot.Content, monster))
            {
                return false;
            }

            monster.Inventory.TransferAllTo(slot.GroundInventory);
            return ApplySlotContentChange(
                room,
                SlotContentChangeRequest.Remove(slot, monster),
                turnResult);
        }

        private bool RemoveDestroyedTreasureChest(
            Room room,
            RoomSlot slot,
            TreasureChest chest,
            TurnResult turnResult)
        {
            if (slot == null || chest == null ||
                !chest.IsDestroyed ||
                !ReferenceEquals(slot.Content, chest))
            {
                return false;
            }

            return ApplySlotContentChange(
                room,
                SlotContentChangeRequest.Remove(slot, chest),
                turnResult);
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

            bool moved = ApplySlotContentChange(
                room,
                SlotContentChangeRequest.Move(
                    sourceSlot,
                    request.TargetSlot,
                    request.Monster),
                turnResult);
            if (!moved)
            {
                return;
            }

            request.Monster.CompleteMove(request.StateAfterMove);
        }

        private static void AddDoorDiscoveryMessage(
            RoomSlot slot,
            TurnResult turnResult)
        {
            if (slot != null && slot.TryDiscoverDoor())
            {
                turnResult.Messages.Add(
                    ExplorationMessages.DoorFound());
            }
        }

        private void ApplySlotContentChanges(
            Room room,
            IEnumerable<SlotContentChangeRequest> requests,
            TurnResult turnResult)
        {
            if (requests == null)
            {
                return;
            }

            foreach (SlotContentChangeRequest request in requests)
            {
                ApplySlotContentChange(
                    room,
                    request,
                    turnResult);
            }
        }

        private bool ApplySlotContentChange(
            Room room,
            SlotContentChangeRequest request,
            TurnResult turnResult)
        {
            if (room == null || request == null ||
                turnResult == null)
            {
                return false;
            }

            switch (request.ChangeType)
            {
                case SlotContentChangeType.Place:
                    return ApplyContentPlace(
                        room,
                        request,
                        turnResult);
                case SlotContentChangeType.Remove:
                    return ApplyContentRemove(
                        room,
                        request,
                        turnResult);
                case SlotContentChangeType.Move:
                    return ApplyContentMove(
                        room,
                        request,
                        turnResult);
                case SlotContentChangeType.Replace:
                    return ApplyContentReplace(
                        room,
                        request,
                        turnResult);
                default:
                    return false;
            }
        }

        private bool ApplyContentPlace(
            Room room,
            SlotContentChangeRequest request,
            TurnResult turnResult)
        {
            RoomSlot target = request.TargetSlot;
            if (!ContainsSlot(room, target) || !target.IsEmpty ||
                request.NewContent == null ||
                ContainsContent(room, request.NewContent))
            {
                return false;
            }

            target.SetContent(request.NewContent);
            AddChangedSlot(room, target, turnResult);
            return true;
        }

        private bool ApplyContentRemove(
            Room room,
            SlotContentChangeRequest request,
            TurnResult turnResult)
        {
            RoomSlot source = request.SourceSlot;
            if (!ContainsSlot(room, source) ||
                request.ExpectedContent == null ||
                !ReferenceEquals(
                    source.Content,
                    request.ExpectedContent))
            {
                return false;
            }

            source.ClearContent();
            AddChangedSlot(room, source, turnResult);
            AddDoorDiscoveryMessage(source, turnResult);
            return true;
        }

        private bool ApplyContentMove(
            Room room,
            SlotContentChangeRequest request,
            TurnResult turnResult)
        {
            RoomSlot source = request.SourceSlot;
            RoomSlot target = request.TargetSlot;
            if (!ContainsSlot(room, source) ||
                !ContainsSlot(room, target) ||
                ReferenceEquals(source, target) ||
                request.ExpectedContent == null ||
                !ReferenceEquals(
                    source.Content,
                    request.ExpectedContent) ||
                !target.IsEmpty)
            {
                return false;
            }

            ISlotContent content = source.TakeContent();
            target.SetContent(content);
            AddChangedSlot(room, source, turnResult);
            AddChangedSlot(room, target, turnResult);
            AddDoorDiscoveryMessage(source, turnResult);
            return true;
        }

        private bool ApplyContentReplace(
            Room room,
            SlotContentChangeRequest request,
            TurnResult turnResult)
        {
            RoomSlot target = request.TargetSlot;
            if (!ContainsSlot(room, target) ||
                request.ExpectedContent == null ||
                !ReferenceEquals(
                    target.Content,
                    request.ExpectedContent) ||
                request.NewContent == null ||
                ContainsContentExcept(
                    room,
                    request.NewContent,
                    target))
            {
                return false;
            }

            target.SetContent(request.NewContent);
            AddChangedSlot(room, target, turnResult);
            return true;
        }

        private static bool ContainsSlot(
            Room room,
            RoomSlot slot)
        {
            return room != null && slot != null &&
                   room.Slots.Contains(slot);
        }

        private static bool ContainsContent(
            Room room,
            ISlotContent content)
        {
            return room != null && content != null &&
                   room.Slots.Exists(slot => ReferenceEquals(
                       slot.Content,
                       content));
        }

        private static bool ContainsContentExcept(
            Room room,
            ISlotContent content,
            RoomSlot excludedSlot)
        {
            return room != null && content != null &&
                   room.Slots.Exists(slot =>
                       !ReferenceEquals(slot, excludedSlot) &&
                       ReferenceEquals(slot.Content, content));
        }

        private void ApplySlotStateChanges(
            Room room,
            IEnumerable<SlotStateChangeRequest> requests,
            TurnResult turnResult)
        {
            if (room == null || requests == null ||
                turnResult == null)
            {
                return;
            }

            foreach (SlotStateChangeRequest request in requests)
            {
                if (request == null || request.Slot == null ||
                    !room.Slots.Contains(request.Slot))
                {
                    continue;
                }

                RoomSlot changedSlot = request.Slot;
                if (changedSlot.State != request.State)
                {
                    changedSlot.SetState(request.State);
                    AddChangedSlot(room, changedSlot, turnResult);
                }

                if (request.State == SlotState.REVEALED)
                {
                    AddDoorDiscoveryMessage(
                        changedSlot,
                        turnResult);
                }
            }
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
