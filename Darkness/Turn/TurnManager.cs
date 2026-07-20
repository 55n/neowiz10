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
            }

            PlayerActionContext playerAction = new PlayerActionContext(
                hero,
                command.Action,
                command.TargetSlot,
                command.Item,
                command.SkillUse,
                command.ItemSourceEquipmentSlot);
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
            if (context.Action == PlayerActionType.UseSkill &&
                context.SkillUse != null)
            {
                result.SkillUses.Add(context.SkillUse);
                ApplyInteraction(
                    room,
                    context.TargetSlot,
                    result,
                    turnResult);
                return;
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
                    turnResult.Messages.Add(
                        GetVisibleDecisionMessage(
                            entry,
                            decision.Message));
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
                slot.ClearContent();
                AddChangedSlot(room, slot, turnResult);
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
            if (DropDefeatedMonsterLoot(slot))
            {
                turnResult.Messages.Add(
                    CombatMessages.MonsterDefeated(
                        defeatedMonsterName));
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

            sourceSlot.Reveal();
            AddChangedSlot(room, sourceSlot, turnResult);
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
            if (source == null || string.IsNullOrEmpty(message))
            {
                return message;
            }

            string visibleName = GetVisibleName(room, source);
            return visibleName == source.Name
                ? message
                : message.Replace(source.Name, visibleName);
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

        private bool DropDefeatedMonsterLoot(RoomSlot slot)
        {
            if (slot == null)
            {
                return false;
            }

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
