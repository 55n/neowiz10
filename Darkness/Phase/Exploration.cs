using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    class Exploration
    {
        private readonly Hero hero;
        private readonly Dungeon dungeon;
        private readonly ExplorationScreen screen;
        private readonly ItemData itemData;
        private readonly MonsterData monsterData;
        private readonly SkillData skillData;
        private readonly TurnManager turnManager;
        private bool openingEventCompleted;

        public Exploration(Hero hero, Dungeon dungeon)
        {
            this.hero = hero;
            this.dungeon = dungeon;
            screen = new ExplorationScreen();
            itemData = new ItemData();
            monsterData = new MonsterData();
            skillData = new SkillData();
            turnManager = new TurnManager();
        }

        public GameSignal Run()
        {
            if (!openingEventCompleted)
            {
                RunOpeningEvent();
                openingEventCompleted = true;
            }

            ApplyRoomEntryEffect(dungeon.CurrentRoom);
            screen.InitScreen(dungeon.CurrentRoom, hero);

            while (true)
            {
                object value = screen.explorationPanel.ReadSelection();

                SkillSelection skillSelection = value as SkillSelection;
                if (skillSelection != null)
                {
                    TurnResult skillResult =
                        HandleSkillSelection(skillSelection);
                    if (skillResult != null)
                    {
                        ApplyTurnResult(skillResult);
                        if (skillResult.HeroDied)
                        {
                            return GameSignal.GAME_OVER;
                        }
                    }
                    continue;
                }

                ScreenSelection selection = value as ScreenSelection;
                if (selection != null)
                {
                    TurnResult selectionResult =
                        HandleScreenSelection(selection);
                    if (selectionResult != null)
                    {
                        ApplyTurnResult(selectionResult);
                        if (selectionResult.HeroDied)
                        {
                            return GameSignal.GAME_OVER;
                        }
                    }
                    continue;
                }

                RoomDirection direction;
                if (TryGetRoomDirection(value, out direction))
                {
                    TurnResult moveResult = turnManager.ResolveMove(
                        dungeon,
                        direction,
                        hero);
                    ApplyTurnResult(moveResult);
                    if (moveResult.HeroDied)
                    {
                        return GameSignal.GAME_OVER;
                    }
                    continue;
                }

                PlayerActionType action;
                if (TryGetPlayerAction(value, out action))
                {
                    TurnResult actionResult =
                        ResolvePlayerAction(action, null);
                    ApplyTurnResult(actionResult);
                    if (actionResult.HeroDied)
                    {
                        return GameSignal.GAME_OVER;
                    }
                }
            }
        }

        private TurnResult ResolvePlayerAction(
            PlayerActionType action,
            ItemStack itemStack,
            EquipmentSlot? itemSourceEquipmentSlot = null)
        {
            RoomSlot slot = null;
            if (PlayerActionRules.RequiresTargetSlot(action))
            {
                int slotIndex = screen.ChooseSlot();
                slot = dungeon.CurrentRoom.Slots[slotIndex];
            }

            PlayerTurnCommand command = new PlayerTurnCommand(
                action,
                slot,
                itemStack,
                null,
                itemSourceEquipmentSlot,
                true);
            return turnManager.Resolve(
                command,
                hero,
                dungeon.CurrentRoom);
        }

        private void ApplyTurnResult(TurnResult result)
        {
            if (!result.RoomChanged &&
                result.ChangedSlotIndexes.Count > 0)
            {
                screen.RefreshRoom();
            }

            if (result.Messages.Count > 0)
            {
                Utility.PlayMessages(result.Messages.ToArray());
            }

            if (result.RoomChanged && !result.HeroDied)
            {
                ApplyRoomEntryEffect(dungeon.CurrentRoom);
                screen.InitScreen(dungeon.CurrentRoom, hero);
                return;
            }

            if (result.TurnCompleted && !result.HeroDied)
            {
                screen.OpenExplorationSelection();
            }
        }

        private bool TryGetRoomDirection(
            object value,
            out RoomDirection direction)
        {
            if (value is ExplorationSelectionOptions &&
                (ExplorationSelectionOptions)value ==
                    ExplorationSelectionOptions.BACK)
            {
                direction = RoomDirection.BACK;
                return true;
            }

            if (value is MoveSelectionOptions)
            {
                switch ((MoveSelectionOptions)value)
                {
                    case MoveSelectionOptions.BACK:
                        direction = RoomDirection.BACK;
                        return true;
                    case MoveSelectionOptions.FORWARD:
                        direction = RoomDirection.FORWARD;
                        return true;
                    case MoveSelectionOptions.LEFT:
                        direction = RoomDirection.LEFT;
                        return true;
                    case MoveSelectionOptions.RIGHT:
                        direction = RoomDirection.RIGHT;
                        return true;
                }
            }

            direction = default(RoomDirection);
            return false;
        }

        private bool TryGetPlayerAction(
            object value,
            out PlayerActionType action)
        {
            if (value is EncounterSelectionOptions)
            {
                EncounterSelectionOptions option =
                    (EncounterSelectionOptions)value;
                if (option == EncounterSelectionOptions.WAIT)
                {
                    action = PlayerActionType.Wait;
                    return true;
                }
                if (option == EncounterSelectionOptions.TALK)
                {
                    action = PlayerActionType.Talk;
                    return true;
                }
                if (option == EncounterSelectionOptions.THROW)
                {
                    action = PlayerActionType.ThrowItem;
                    return true;
                }
                if (option == EncounterSelectionOptions.SEARCH)
                {
                    action = PlayerActionType.Search;
                    return true;
                }
            }

            if (value is AttackSelectionOptions)
            {
                AttackSelectionOptions option =
                    (AttackSelectionOptions)value;
                if (option == AttackSelectionOptions.ATTACK)
                {
                    action = PlayerActionType.Attack;
                    return true;
                }
                if (option == AttackSelectionOptions.DEFFENSE)
                {
                    action = PlayerActionType.Defend;
                    return true;
                }
            }

            action = default(PlayerActionType);
            return false;
        }

        private void RunOpeningEvent()
        {
            MonsterType fallType = monsterData.MonsterTypes["fall"];
            Monster fall = new Monster(
                fallType,
                new Inventory(0),
                new List<ActiveEffect>(),
                new DefaultMonsterBehavior(),
                new List<string>());

            AttackContext fallAttack = new AttackContext(
                fall,
                hero,
                fallType.Attack,
                fallType.Accuracy,
                hero.Type.Evasion,
                AttackDeliveryType.Trap,
                null,
                0);
            AttackResult fallResult = AttackResolver.Resolve(fallAttack);
            DurabilityResolveResult durabilityResult =
                new DurabilityResolver().ResolveAttack(
                    fallAttack,
                    fallResult,
                    dungeon.CurrentRoom);
            Utility.PlayMessages(durabilityResult.Messages.ToArray());

            Utility.PlayMessage(CombatMessages.DamageTaken(
                fallType.Name,
                fallType.Attack.ToString()));
            Utility.PlayMessage(SkillMessages.Activated("수호의 가호"));

            TransformGuardianCharm();
            Utility.PlayMessage(EquipmentMessages.Broken("수호의 부적"));

            Utility.PlayMessages(ExplorationMessages.FirstRoomScript());
            GiveAndEquipSword();
        }

        private void TransformGuardianCharm()
        {
            ItemStack charm;
            if (!hero.Equipment.TryGetValue(EquipmentSlot.Accessory, out charm) ||
                charm == null)
            {
                return;
            }

            charm.Item.Transform(
                itemData.ItemTypes["cracked_guardian_charm"]);
        }

        private void GiveAndEquipSword()
        {
            ItemType swordType = itemData.ItemTypes["worn_sword"];
            hero.Inventory.Store(new ItemStack(new Item(swordType), 1));

            ItemStack sword = hero.Inventory.ItemStacks.Find(
                itemStack => itemStack.Item.Type.Id == swordType.Id);
            hero.Equip(EquipmentSlot.Weapon, sword);

            Utility.PlayMessage(InventoryMessages.ItemObtained(swordType.Name));
        }

        private TurnResult HandleScreenSelection(ScreenSelection selection)
        {
            if (selection.Action == ScreenAction.EquipItem &&
                selection.EquipmentSlot.HasValue)
            {
                string itemName = selection.ItemStack == null
                    ? "장비"
                    : selection.ItemStack.Item.Type.Name;
                bool equipped = hero.Equip(
                    selection.EquipmentSlot.Value,
                    selection.ItemStack);
                Utility.PlayMessage(
                    equipped
                        ? EquipmentMessages.Equipped(itemName)
                        : EquipmentMessages.CannotEquip());
                screen.OpenExplorationSelection();
                return null;
            }
            else if (selection.Action == ScreenAction.UnequipItem &&
                     selection.EquipmentSlot.HasValue)
            {
                string itemName = selection.ItemStack == null
                    ? "장비"
                    : selection.ItemStack.Item.Type.Name;
                bool unequipped = hero.Unequip(
                    selection.EquipmentSlot.Value);
                Utility.PlayMessage(
                    unequipped
                        ? EquipmentMessages.Unequipped(itemName)
                        : EquipmentMessages.CannotUnequip());
                screen.OpenExplorationSelection();
                return null;
            }
            else if (selection.Action == ScreenAction.ThrowItem)
            {
                if (selection.ItemStack != null &&
                    selection.ItemStack.Count > 0)
                {
                    return ResolvePlayerAction(
                        PlayerActionType.ThrowItem,
                        selection.ItemStack,
                        selection.EquipmentSlot);
                }
            }

            return null;
        }

        private TurnResult HandleSkillSelection(SkillSelection selection)
        {
            SkillType skill;
            if (selection == null ||
                !skillData.SkillTypes.TryGetValue(
                    selection.SkillId,
                    out skill))
            {
                screen.OpenExplorationSelection();
                return null;
            }

            if (skill.Id == "cowardly_leap")
            {
                return ResolveCowardlyLeap(skill);
            }

            RoomSlot targetSlot = null;
            IEnumerable<object> selectedTargets = null;
            if (skill.TargetingType == SkillTargetingType.SingleSlot)
            {
                int slotIndex = screen.ChooseSlot();
                targetSlot = dungeon.CurrentRoom.Slots[slotIndex];
                if (targetSlot.Content != null)
                {
                    selectedTargets = new object[]
                    {
                        targetSlot.Content
                    };
                }
            }
            else if (skill.TargetingType != SkillTargetingType.None)
            {
                screen.OpenExplorationSelection();
                return null;
            }

            SkillUseContext skillUse = new SkillUseContext(
                hero,
                skill,
                dungeon.CurrentRoom,
                selectedTargets);
            PlayerTurnCommand command = new PlayerTurnCommand(
                PlayerActionType.UseSkill,
                targetSlot,
                null,
                skillUse,
                true);
            return turnManager.Resolve(
                command,
                hero,
                dungeon.CurrentRoom);
        }

        private TurnResult ResolveCowardlyLeap(SkillType skill)
        {
            TurnResult result = new TurnResult();
            if (!dungeon.Rooms.ContainsKey("room-28") ||
                !SkillCostResolver.TryPay(hero, skill))
            {
                result.Messages.Add(
                    SkillMessages.CannotUse(skill.Name));
                return result;
            }

            dungeon.MoveTo("room-28");
            result.Messages.Add(
                SkillMessages.Used(hero.Name, skill.Name));
            result.RoomChanged = true;
            result.TurnCompleted = true;
            return result;
        }

        private void ApplyRoomEntryEffect(Room room)
        {
            if (room == null || room.Type.Id != "room-4")
            {
                return;
            }

            hero.RestoreHealth(hero.Type.MaxHealth);
            hero.RestoreFocus(hero.Type.MaxFocus);
        }

    }
}
