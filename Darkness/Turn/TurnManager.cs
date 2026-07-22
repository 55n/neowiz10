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

        public TurnResult RevealAllSlots(Room room)
        {
            if (room == null)
            {
                throw new ArgumentNullException("room");
            }

            TurnResult result = new TurnResult();
            List<SlotStateChangeRequest> requests =
                new List<SlotStateChangeRequest>();
            foreach (RoomSlot slot in room.Slots)
            {
                if (slot.State != SlotState.REVEALED)
                {
                    requests.Add(
                        SlotStateChangeRequest.Reveal(slot));
                }
            }

            ApplySlotStateChanges(room, requests, result);
            return result;
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
                ValidateTarget(command, room);
                turnResult.TurnNumber = TurnNumber;
                return turnResult;
            }

            string bindingName;
            int remainingBindingStacks;
            if (EffectActionRules.TryConsumeForcedWait(
                    hero,
                    out bindingName,
                    out remainingBindingStacks))
            {
                command = new PlayerTurnCommand(
                    PlayerActionType.Wait,
                    null,
                    null,
                    null,
                    null,
                    true,
                    false);
                turnResult.Messages.Add(
                    EffectMessages.ForcedWait(
                        hero.Name,
                        bindingName,
                        remainingBindingStacks));
            }

            ValidateTarget(command, room);

            List<ActiveEffect> expiringPlayerEffects =
                CaptureExpiringPlayerEffects(hero);

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
                command.ItemSourceEquipmentSlot,
                room);
            string playerActionMessage = command.AnnouncesAction
                ? HeroActionMessages.Started(
                    command.Action,
                    GetVisibleSlotTargetName(command.TargetSlot))
                : null;
            if (!string.IsNullOrEmpty(playerActionMessage))
            {
                turnResult.Messages.Add(playerActionMessage);
            }

            List<SkillOutcomeReflectionEntry> reflections =
                BeginSkillOutcomeReflections(room, playerAction);
            bool loudEventOccurred = ApplyPlayerAction(
                room,
                playerAction,
                turnResult);
            CompleteSkillOutcomeReflections(reflections);
            RemoveExpiredPlayerEffects(
                hero,
                expiringPlayerEffects);

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
            ExecuteRoomTurnBehavior(
                room,
                hero,
                playerAction,
                turnResult);
            if (isDefending)
            {
                hero.RemoveEffect(defendingEffectType.Id);
            }

            Phase = TurnPhase.EndTurn;
            ApplyRoomEffects(room, hero, turnResult);
            ResolveTurnEndEffects(room, hero, turnResult);
            RemoveEndTurnDefeatedMonsters(room, turnResult);
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
            if (EffectActionRules.HasForcedWait(hero))
            {
                return Resolve(
                    new PlayerTurnCommand(
                        PlayerActionType.Wait,
                        null,
                        null,
                        null,
                        null,
                        true,
                        false),
                    hero,
                    room);
            }

            TurnResult interceptionResult;
            if (TryInterceptMove(
                    room,
                    direction,
                    hero,
                    out interceptionResult))
            {
                return interceptionResult;
            }

            TurnResult turnResult = new TurnResult();
            List<ActiveEffect> expiringPlayerEffects =
                CaptureExpiringPlayerEffects(hero);

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
            ExecuteRoomTurnBehavior(
                room,
                hero,
                playerAction,
                turnResult);
            RemoveExpiredPlayerEffects(
                hero,
                expiringPlayerEffects);
            hero.RemoveEffect(hastyEffectType.Id);

            Phase = TurnPhase.EndTurn;
            ApplyRoomEffects(room, hero, turnResult);
            ResolveTurnEndEffects(room, hero, turnResult);
            RemoveEndTurnDefeatedMonsters(room, turnResult);
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

        private bool TryInterceptMove(
            Room room,
            RoomDirection direction,
            Hero hero,
            out TurnResult turnResult)
        {
            turnResult = null;
            if (room == null || room.MoveInterceptor == null)
            {
                return false;
            }

            RoomEdge edge;
            if (!room.Edges.TryGetValue(direction, out edge))
            {
                return false;
            }

            MoveInterceptionResult interception =
                room.MoveInterceptor.TryIntercept(
                    new MoveInterceptionContext(
                        hero,
                        room,
                        direction,
                        edge));
            if (interception == null ||
                !interception.MovementCanceled)
            {
                return false;
            }

            turnResult = new TurnResult();
            Phase = TurnPhase.PlayerAction;
            ApplyInteraction(
                room,
                null,
                interception.Interaction,
                turnResult);
            turnResult.TurnCompleted = true;
            turnResult.TurnNumber = TurnNumber;
            turnResult.HeroDied = !hero.CanAct;
            Phase = TurnPhase.PlayerInput;
            return true;
        }

        public TurnResult SynchronizeRoomEffects(Room room, Hero hero)
        {
            if (room == null)
            {
                throw new ArgumentNullException("room");
            }

            if (hero == null)
            {
                throw new ArgumentNullException("hero");
            }

            TurnResult result = new TurnResult();
            foreach (ActiveEffect effect in hero.Effects.ToArray())
            {
                if (effect.IsActiveInRoom(room))
                {
                    continue;
                }

                hero.Effects.Remove(effect);
                result.Messages.Add(HeroStateMessages.EffectEnded(
                    hero.Name,
                    effect.Type.Name));
            }

            ApplyRoomEffects(room, hero, result);
            return result;
        }

        private static List<ActiveEffect> CaptureExpiringPlayerEffects(
            Hero hero)
        {
            return hero.Effects.FindAll(
                effect => effect.Type.Duration == 1);
        }

        private static void RemoveExpiredPlayerEffects(
            Hero hero,
            IEnumerable<ActiveEffect> effects)
        {
            foreach (ActiveEffect effect in effects)
            {
                hero.Effects.Remove(effect);
            }
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

        private static List<SkillOutcomeReflectionEntry>
            BeginSkillOutcomeReflections(
                Room room,
                PlayerActionContext playerAction)
        {
            List<SkillOutcomeReflectionEntry> entries =
                new List<SkillOutcomeReflectionEntry>();
            if (playerAction == null ||
                playerAction.Action != PlayerActionType.UseSkill)
            {
                return entries;
            }

            foreach (RoomSlot slot in room.Slots)
            {
                Monster monster = slot.Content as Monster;
                ISkillOutcomeReflector reflector = monster == null
                    ? null
                    : monster.Behavior as ISkillOutcomeReflector;
                if (reflector == null)
                {
                    continue;
                }

                reflector.BeginSkillResolution(monster);
                entries.Add(new SkillOutcomeReflectionEntry(
                    monster,
                    reflector));
            }

            return entries;
        }

        private static void CompleteSkillOutcomeReflections(
            IEnumerable<SkillOutcomeReflectionEntry> entries)
        {
            foreach (SkillOutcomeReflectionEntry entry in entries)
            {
                entry.Reflector.CompleteSkillResolution(
                    entry.Monster);
            }
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
            Body bodyBeforeAction = slot == null
                ? null
                : slot.Content as Body;
            Trap trapBeforeAction = slot == null
                ? null
                : slot.Content as Trap;
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
                            skillUse,
                            message));
                }
                result.Attacks.AddRange(skillResult.Attacks);
                result.Damages.AddRange(skillResult.Damages);
                result.DurabilityRequests.AddRange(
                    skillResult.DurabilityRequests);
                result.MonsterMoves.AddRange(
                    skillResult.MonsterMoves);
            }

            foreach (EffectRequest request in
                     result.EffectRequests.ToArray())
            {
                ResolveEffectRequest(
                    request,
                    result,
                    room,
                    turnResult);
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
                ActiveEffect triggeredEffect =
                    damage.Source as ActiveEffect;
                if (triggeredEffect != null &&
                    triggeredEffect.Type != null)
                {
                    turnResult.Messages.Add(
                        EffectMessages.Triggered(
                            GetVisibleName(room, damage.Target),
                            triggeredEffect.Type.Name));
                }

                List<ActiveEffect> sourceEffects =
                    CaptureEffects(damage.Source as IDamageable);
                List<ActiveEffect> targetEffects =
                    CaptureEffects(damage.Target);
                DamageResolver.Resolve(damage);
                AddConsumedEffectMessages(
                    room,
                    damage.Source as IDamageable,
                    sourceEffects,
                    turnResult);
                AddConsumedEffectMessages(
                    room,
                    damage.Target,
                    targetEffects,
                    turnResult);
                turnResult.Messages.Add(
                    CombatMessages.DamageReceived(
                        GetVisibleName(room, damage.Target),
                        damage.FinalDamage));
            }

            foreach (EffectCopyRequest copy in result.EffectCopies)
            {
                ApplyEffectCopy(copy, room, turnResult);
            }

            foreach (FocusDamageRequest focusDamage in
                     result.FocusDamages)
            {
                RoomSlot sourceSlot = FindContentSlot(
                    room,
                    focusDamage.Source);
                if (sourceSlot != null &&
                    sourceSlot.State != SlotState.REVEALED)
                {
                    ApplySlotStateChanges(
                        room,
                        new[]
                        {
                            SlotStateChangeRequest.Reveal(sourceSlot)
                        },
                        turnResult);
                }

                int previousFocus = focusDamage.Target.CurrentFocus;
                focusDamage.Target.SpendFocus(focusDamage.Amount);
                int drainedFocus = previousFocus -
                    focusDamage.Target.CurrentFocus;
                turnResult.Messages.Add(
                    CombatMessages.FocusDrained(
                        focusDamage.Target.Name,
                        drainedFocus));
            }

            RemoveDestroyedSlotContents(
                room,
                turnResult);

            Monster defeatedMonster = slot == null
                ? null
                : slot.Content as Monster;
            string defeatedMonsterName =
                defeatedMonster != null && !defeatedMonster.IsAlive
                    ? GetVisibleName(room, defeatedMonster)
                    : null;
            bool defeatedMonsterHadLoot =
                defeatedMonster != null &&
                defeatedMonster.Inventory.ItemStacks.Count > 0;
            if (FinalizeDefeatedMonster(
                    room,
                    slot,
                    defeatedMonster,
                    turnResult))
            {
                turnResult.Messages.Add(
                    CombatMessages.MonsterDefeated(
                        defeatedMonsterName));
                if (defeatedMonsterHadLoot)
                {
                    turnResult.Messages.Add(
                        CombatMessages.MonsterLootDropped());
                }
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

            RemoveDestroyedBody(
                room,
                slot,
                bodyBeforeAction,
                turnResult);
            RemoveDestroyedTrap(
                room,
                slot,
                trapBeforeAction,
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

        private static void ApplyEffectCopy(
            EffectCopyRequest request,
            Room room,
            TurnResult turnResult)
        {
            if (request == null || request.Target == null ||
                request.StackCount <= 0)
            {
                return;
            }

            ActiveEffectFactory factory = new ActiveEffectFactory();
            for (int i = 0; i < request.StackCount; i++)
            {
                ActiveEffect effect = factory.Create(
                    request.EffectId,
                    request.Source);
                if (effect == null || !effect.CanApplyTo(request.Target))
                {
                    continue;
                }

                request.Target.ApplyEffect(effect);
            }

            ActiveEffect applied = request.Target.Effects.Find(
                effect => effect.Type.Id == request.EffectId);
            if (applied != null)
            {
                turnResult.Messages.Add(
                    EffectMessages.StackApplied(
                        GetVisibleEffectTargetName(room, request.Target),
                        applied.Type.Name,
                        applied.StackCount));
            }
        }

        private void ExecuteRoomTurnBehavior(
            Room room,
            Hero hero,
            PlayerActionContext playerAction,
            TurnResult turnResult)
        {
            if (room == null || room.TurnBehavior == null ||
                hero == null || !hero.CanAct)
            {
                return;
            }

            SlotInteractionResult result =
                room.TurnBehavior.Act(
                    room,
                    hero,
                    playerAction);
            if (result != null)
            {
                ApplyInteraction(
                    room,
                    null,
                    result,
                    turnResult);
            }
        }

        private void ResolveEffectRequest(
            EffectRequest request,
            SlotInteractionResult result,
            Room room,
            TurnResult turnResult)
        {
            if (request == null || result == null ||
                turnResult == null)
            {
                return;
            }

            EffectPlan plan;
            if (!effectResolver.TryPrepare(
                    request.Applications,
                    request.Context,
                    out plan))
            {
                return;
            }

            EffectResolveResult effectResult =
                effectResolver.Execute(plan);
            result.Attacks.AddRange(effectResult.Attacks);
            result.Damages.AddRange(effectResult.Damages);
            result.DurabilityRequests.AddRange(
                effectResult.DurabilityRequests);
            AddAppliedEffectMessages(
                room,
                effectResult,
                turnResult);
        }

        private static void AddAppliedEffectMessages(
            Room room,
            EffectResolveResult effectResult,
            TurnResult turnResult)
        {
            List<AppliedEffectResult> latestResults =
                new List<AppliedEffectResult>();
            foreach (AppliedEffectResult applied in
                     effectResult.AppliedEffects)
            {
                if (applied == null || applied.Target == null ||
                    applied.Effect == null)
                {
                    continue;
                }

                int existingIndex = latestResults.FindIndex(
                    existing =>
                        ReferenceEquals(
                            existing.Target,
                            applied.Target) &&
                        existing.Effect.Id == applied.Effect.Id);
                if (existingIndex >= 0)
                {
                    latestResults[existingIndex] = applied;
                }
                else
                {
                    latestResults.Add(applied);
                }
            }

            foreach (AppliedEffectResult applied in latestResults)
            {
                turnResult.Messages.Add(
                    EffectMessages.StackApplied(
                        GetVisibleEffectTargetName(
                            room,
                            applied.Target),
                        applied.Effect.Name,
                        applied.StackCount));
            }
        }

        private class SkillOutcomeReflectionEntry
        {
            public Monster Monster { get; private set; }
            public ISkillOutcomeReflector Reflector { get; private set; }

            public SkillOutcomeReflectionEntry(
                Monster monster,
                ISkillOutcomeReflector reflector)
            {
                Monster = monster;
                Reflector = reflector;
            }
        }

        private static string GetVisibleEffectTargetName(
            Room room,
            IEffectTarget target)
        {
            RoomSlot slot = FindContentSlot(room, target);
            return slot != null &&
                   slot.State != SlotState.REVEALED
                ? "???"
                : target.Name;
        }

        private void ResolveItemThrow(
            Room room,
            ItemThrowPlan plan,
            SlotInteractionResult interactionResult,
            TurnResult turnResult)
        {
            if (plan == null)
            {
                return;
            }

            if (plan.ImpactAttack != null)
            {
                AttackResult attackResult = ResolveAttack(
                    room,
                    plan.ImpactAttack,
                    turnResult);
                if (!attackResult.IsHit ||
                    plan.Target.CurrentHealth <= 0)
                {
                    return;
                }
            }

            if (plan.EffectTarget == null ||
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
            AddAppliedEffectMessages(
                room,
                effectResult,
                turnResult);
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

            List<ActiveEffect> sourceEffects =
                CaptureEffects(attack.Source);
            List<ActiveEffect> targetEffects =
                CaptureEffects(attack.Target);
            AttackResult attackResult = AttackResolver.Resolve(attack);
            AddConsumedEffectMessages(
                room,
                attack.Source,
                sourceEffects,
                turnResult);
            AddConsumedEffectMessages(
                room,
                attack.Target,
                targetEffects,
                turnResult);
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

        private static List<ActiveEffect> CaptureEffects(
            IDamageable target)
        {
            return target == null || target.Effects == null
                ? new List<ActiveEffect>()
                : new List<ActiveEffect>(target.Effects);
        }

        private static void AddConsumedEffectMessages(
            Room room,
            IDamageable target,
            IEnumerable<ActiveEffect> effectsBeforeResolution,
            TurnResult turnResult)
        {
            if (target == null || effectsBeforeResolution == null)
            {
                return;
            }

            foreach (ActiveEffect effect in effectsBeforeResolution)
            {
                if (effect == null || effect.Type == null ||
                    target.Effects.Contains(effect))
                {
                    continue;
                }

                turnResult.Messages.Add(
                    EffectMessages.Consumed(
                        GetVisibleName(room, target),
                        effect.Type.Name));
            }
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
            string actor = entry.Slot.State == SlotState.REVEALED
                ? entry.Monster.Name
                : "???";
            return NarrativeTemplate.Format(message, actor);
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
            SkillUseContext skillUse,
            string message)
        {
            IDamageable source = skillUse.User as IDamageable;
            string actor = source == null
                ? skillUse.User.Name
                : GetVisibleName(room, source);
            object selectedTarget = skillUse.SelectedTargets.Count == 0
                ? null
                : skillUse.SelectedTargets[0];
            string target = GetVisibleTemplateTargetName(
                room,
                selectedTarget);
            return NarrativeTemplate.Format(message, actor, target);
        }

        private static string GetVisibleTemplateTargetName(
            Room room,
            object target)
        {
            if (target == null)
            {
                return "";
            }

            RoomSlot slot = FindContentSlot(room, target);
            if (slot != null && slot.State != SlotState.REVEALED)
            {
                return "???";
            }

            IEffectTarget effectTarget = target as IEffectTarget;
            return effectTarget == null
                ? target.ToString()
                : effectTarget.Name;
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

        private void ApplyRoomEffects(
            Room room,
            Hero hero,
            TurnResult turnResult)
        {
            if (room == null || hero == null || turnResult == null)
            {
                return;
            }

            foreach (RoomSlot slot in room.Slots.ToArray())
            {
                IRoomEffectSource source =
                    slot.Content as IRoomEffectSource;
                if (source == null || !source.IsRoomEffectActive)
                {
                    continue;
                }

                SlotInteractionResult interaction =
                    new SlotInteractionResult();
                interaction.EffectRequests.Add(new EffectRequest(
                    source.RoomEffects,
                    EffectContext.FromRoomEffect(
                        source,
                        room,
                        hero)));
                ApplyInteraction(
                    room,
                    null,
                    interaction,
                    turnResult);
            }
        }

        private static void ResolveTurnEndEffects(
            Room room,
            Hero hero,
            TurnResult turnResult)
        {
            List<IEffectTarget> targets =
                CollectTurnEffectTargets(room, hero);
            foreach (IEffectTarget target in targets)
            {
                IDamageable damageable = target as IDamageable;
                if (damageable == null)
                {
                    continue;
                }

                EffectTurnContext context =
                    new EffectTurnContext(room, damageable);
                foreach (ActiveEffect effect in target.Effects.ToArray())
                {
                    effect.OnTurnEnd(context);
                }

                foreach (DamageContext damage in context.Damages)
                {
                    DamageResolver.Resolve(damage);
                    ActiveEffect sourceEffect =
                        damage.Source as ActiveEffect;
                    string effectName = sourceEffect == null ||
                                        sourceEffect.Type == null
                        ? "상태이상"
                        : sourceEffect.Type.Name;
                    turnResult.Messages.Add(
                        HeroStateMessages.EffectDamage(
                            damage.Target.Name,
                            effectName,
                            damage.FinalDamage));
                }

                foreach (ActiveEffect effect in
                         context.EffectsToRemove)
                {
                    if (target.Effects.Remove(effect))
                    {
                        turnResult.Messages.Add(
                            HeroStateMessages.EffectEnded(
                                target.Name,
                                effect.Type.Name));
                    }
                }
            }
        }

        private static List<IEffectTarget> CollectTurnEffectTargets(
            Room room,
            Hero hero)
        {
            List<IEffectTarget> targets = new List<IEffectTarget>();
            if (hero != null)
            {
                targets.Add(hero);
            }

            if (room == null)
            {
                return targets;
            }

            foreach (RoomSlot slot in room.Slots)
            {
                IEffectTarget target = slot.Content as IEffectTarget;
                if (target != null && !targets.Contains(target))
                {
                    targets.Add(target);
                }
            }

            return targets;
        }

        private void RemoveEndTurnDefeatedMonsters(
            Room room,
            TurnResult turnResult)
        {
            if (room == null)
            {
                return;
            }

            foreach (RoomSlot slot in room.Slots.ToArray())
            {
                Monster monster = slot.Content as Monster;
                if (monster == null || monster.IsAlive)
                {
                    continue;
                }

                string name = GetVisibleName(room, monster);
                bool hadLoot = monster.Inventory.ItemStacks.Count > 0;
                if (!FinalizeDefeatedMonster(
                        room,
                        slot,
                        monster,
                        turnResult))
                {
                    continue;
                }

                turnResult.Messages.Add(
                    CombatMessages.MonsterDefeated(name));
                if (hadLoot)
                {
                    turnResult.Messages.Add(
                        CombatMessages.MonsterLootDropped());
                }
            }
        }

        private bool RemoveDestroyedBody(
            Room room,
            RoomSlot slot,
            Body body,
            TurnResult turnResult)
        {
            if (slot == null || body == null ||
                body.CurrentHealth > 0 ||
                !ReferenceEquals(slot.Content, body))
            {
                return false;
            }

            bool hadItems = body.Inventory.ItemStacks.Count > 0;
            bool removed = ApplySlotContentChange(
                room,
                SlotContentChangeRequest.Remove(slot, body),
                turnResult);
            if (!removed)
            {
                return false;
            }

            turnResult.Messages.Add(
                CombatMessages.ObjectDestroyed(body.Name));
            if (hadItems)
            {
                turnResult.Messages.Add(
                    BodyMessages.ItemsDestroyed());
            }

            return true;
        }

        private bool RemoveDestroyedTrap(
            Room room,
            RoomSlot slot,
            Trap trap,
            TurnResult turnResult)
        {
            if (slot == null || trap == null ||
                trap.CurrentHealth > 0 ||
                !ReferenceEquals(slot.Content, trap))
            {
                return false;
            }

            bool removed = ApplySlotContentChange(
                room,
                SlotContentChangeRequest.Remove(slot, trap),
                turnResult);
            if (removed)
            {
                turnResult.Messages.Add(
                    CombatMessages.ObjectDestroyed(trap.Name));
            }

            return removed;
        }

        private bool DropDefeatedMonsterLoot(
            Room room,
            RoomSlot slot,
            Monster monster,
            TurnResult turnResult,
            ISlotContent defeatReplacement = null)
        {
            if (slot == null || monster == null || monster.IsAlive ||
                !ReferenceEquals(slot.Content, monster))
            {
                return false;
            }

            monster.Inventory.TransferAllTo(slot.GroundInventory);

            ISlotContent replacement = defeatReplacement;
            if (replacement == null)
            {
                ISlotMovementBehavior movementBehavior =
                    monster.Behavior as ISlotMovementBehavior;
                replacement = movementBehavior == null
                    ? null
                    : movementBehavior.CreateContentForVacatedSlot(slot);
            }

            return ApplySlotContentChange(
                room,
                replacement == null
                    ? SlotContentChangeRequest.Remove(slot, monster)
                    : SlotContentChangeRequest.Replace(
                        slot,
                        monster,
                        replacement),
                turnResult);
        }

        private bool FinalizeDefeatedMonster(
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

            IDefeatBehavior defeatBehavior =
                monster.Behavior as IDefeatBehavior;
            ISlotContent replacement = null;
            if (defeatBehavior != null)
            {
                DefeatBehaviorResult result =
                    defeatBehavior.ResolveDefeat(monster);
                if (result != null)
                {
                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        turnResult.Messages.Add(result.Message);
                    }

                    if (result.PreventRemoval)
                    {
                        return false;
                    }

                    replacement = result.ReplacementContent;
                }
            }

            return DropDefeatedMonsterLoot(
                room,
                slot,
                monster,
                turnResult,
                replacement);
        }

        private void RemoveDestroyedSlotContents(
            Room room,
            TurnResult turnResult)
        {
            if (room == null || turnResult == null)
            {
                return;
            }

            foreach (RoomSlot slot in room.Slots.ToArray())
            {
                IDestroyableSlotContent content =
                    slot.Content as IDestroyableSlotContent;
                if (content == null || !content.IsDestroyed)
                {
                    continue;
                }

                SlotDestructionResult destruction =
                    content.ResolveDestruction(room, slot) ??
                    new SlotDestructionResult();
                bool removed = ApplySlotContentChange(
                    room,
                    SlotContentChangeRequest.Remove(
                        slot,
                        content),
                    turnResult);
                if (!removed)
                {
                    continue;
                }

                destruction.Drops.TransferAllTo(
                    slot.GroundInventory);
                turnResult.Messages.AddRange(
                    destruction.Messages);
                ApplySlotContentChanges(
                    room,
                    destruction.ContentChanges,
                    turnResult);
            }
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
                !request.Monster.CanAct || request.TargetSlot == null)
            {
                return;
            }

            ISlotMovementBehavior movementBehavior =
                request.Monster.Behavior as ISlotMovementBehavior;
            bool canEnter = request.TargetSlot.IsEmpty ||
                movementBehavior != null &&
                movementBehavior.CanEnter(request.TargetSlot.Content);
            if (!canEnter)
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

            ISlotContent sourceReplacement = movementBehavior == null
                ? null
                : movementBehavior.CreateContentForVacatedSlot(sourceSlot);
            bool moved = ApplySlotContentChange(
                room,
                sourceReplacement == null
                    ? SlotContentChangeRequest.Move(
                        sourceSlot,
                        request.TargetSlot,
                        request.Monster)
                    : SlotContentChangeRequest.MoveReplacing(
                        sourceSlot,
                        request.TargetSlot,
                        request.Monster,
                        sourceReplacement),
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
                request.NewContent == null && !target.IsEmpty)
            {
                return false;
            }

            ISlotContent content = source.TakeContent();
            if (request.NewContent != null)
            {
                source.SetContent(request.NewContent);
            }
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
