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
        private readonly TurnManager turnManager;
        private bool openingEventCompleted;

        public Exploration(Hero hero, Dungeon dungeon)
        {
            this.hero = hero;
            this.dungeon = dungeon;
            screen = new ExplorationScreen();
            itemData = new ItemData();
            monsterData = new MonsterData();
            turnManager = new TurnManager();
        }

        public GameSignal Run()
        {
            if (!openingEventCompleted)
            {
                RunOpeningEvent();
                openingEventCompleted = true;
            }

            screen.InitScreen(dungeon.CurrentRoom, hero);

            while (true)
            {
                object value = screen.explorationPanel.ReadSelection();

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
            ItemStack itemStack)
        {
            int slotIndex = screen.ChooseSlot();
            RoomSlot slot = dungeon.CurrentRoom.Slots[slotIndex];
            PlayerTurnCommand command = new PlayerTurnCommand(
                action,
                slot,
                itemStack,
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
                if (option == AttackSelectionOptions.SKILL)
                {
                    action = PlayerActionType.UseSkill;
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
                new DefaultMonsterBehavior());

            AttackResolver.Resolve(new AttackContext(
                fall,
                hero,
                fallType.Attack,
                fallType.Accuracy,
                hero.Type.Evasion));

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
            ItemType swordType = itemData.ItemTypes["ordinary_sword"];
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
                ItemStack thrownItem = TakeThrownItem(selection);
                if (thrownItem != null)
                {
                    return ResolvePlayerAction(
                        PlayerActionType.ThrowItem,
                        thrownItem);
                }
            }

            return null;
        }

        private ItemStack TakeThrownItem(ScreenSelection selection)
        {
            ItemStack source = selection.ItemStack;
            if (source == null || source.Count <= 0)
            {
                return null;
            }

            ItemStack thrownItem = new ItemStack(source.Item, 1);
            if (selection.EquipmentSlot.HasValue)
            {
                EquipmentSlot slot = selection.EquipmentSlot.Value;
                ItemStack equipped;
                if (!hero.Equipment.TryGetValue(slot, out equipped) ||
                    !ReferenceEquals(equipped, source))
                {
                    return null;
                }

                hero.Equipment[slot] = null;
                return thrownItem;
            }

            return hero.Inventory.Discard(source, 1) == 1
                ? thrownItem
                : null;
        }
    }
}
