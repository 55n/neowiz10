using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Darkness
{
    enum ExplorationSelectionOptions 
    {
        STATUS, INVENTORY, ENCOUNTER, MOVE, BACK    
    }
    enum EncounterSelectionOptions
    {
        WAIT, TALK, THROW, SEARCH, BATTLE, CANCEL
    }
    enum AttackSelectionOptions
    {
        ATTACK, DEFFENSE, CANCEL
    }
    enum MoveSelectionOptions
    {
        BACK, FORWARD, LEFT, RIGHT
    }

    public enum SlotState
    {
        REVEALED, UNREVEALED
    }

    public class ExplorationScreen
    {
        private const int SlotCount = 5;
        private const int SlotWidth = 10;
        private const int SlotGap = 2;
        private const int SlotHeight = 7;
        private const int Top = 2;
        public MessagePanel explorationPanel { get; private set; }


        private Room currentRoom;
        private SelectionNode explorationNode;
        private SelectionOption moveOption;

        public ExplorationScreen()
        {
        }

        public void InitScreen(Room room, Hero hero)
        {
            currentRoom = room;
            explorationPanel = new MessagePanel(
                View.Message,
                BuildExplorationSelection(hero),
                0,
                2);

            RefreshMoveOption();
            View.Display.Clear();
            explorationPanel.ClearSelection();

            if (!room.HasBeenEntered)
            {
                explorationPanel.PlayNarrations(
                    room.Type.EnterMessages);
                room.MarkAsEntered();
            }

            PlayMonsterEncounterMessages(room);

            DrawSlots(View.Display, room.Slots);

            explorationPanel.OpenSelection(
                explorationNode);
        }

        private void PlayMonsterEncounterMessages(Room room)
        {
            foreach (RoomSlot slot in room.Slots)
            {
                Monster monster = slot.Content as Monster;
                if (monster != null && monster.IsAlive)
                {
                    explorationPanel.PlayNarrations(
                        monster.EncounterMessages);
                }
            }
        }

        public SelectionMenu BuildExplorationSelection(Hero hero)
        {
            SkillData skillData = new SkillData();
            string roomDescription = currentRoom == null
                ? ""
                : currentRoom.Type.Description;

            explorationNode = new SelectionNode(
                "exploration-node",
                roomDescription,
                new List<SelectionOption>(),
                null);

            SelectionNode encounterNode = new SelectionNode(
                "encounter-node",
                "",
                new List<SelectionOption>(),
                explorationNode);

            SelectionNode battleNode = new SelectionNode(
                "battle-node",
                "",
                new List<SelectionOption>(),
                encounterNode);

            SelectionNode moveNode = new SelectionNode(
                "move-node",
                "",
                new List<SelectionOption>(),
                explorationNode);

            explorationNode.Options.Add(SelectionOption.DynamicNode(
                "상태창",
                "",
                true,
                () => StatusScreen.BuildNode(hero, explorationNode)));
            explorationNode.Options.Add(SelectionOption.DynamicNode(
                "소지품",
                "",
                true,
                () => InventoryScreen.BuildNode(hero, explorationNode)));
            explorationNode.Options.Add(new SelectionOption(
                "행동", "", true, encounterNode, ExplorationSelectionOptions.ENCOUNTER));
            moveOption = new SelectionOption(
                "이동", "", false, moveNode, ExplorationSelectionOptions.MOVE);
            explorationNode.Options.Add(moveOption);
            explorationNode.Options.Add(new SelectionOption(
                "뒤로가기", "이전 방으로 돌아간다",
                HasAvailableEdge(RoomDirection.BACK),
                null,
                ExplorationSelectionOptions.BACK));

            encounterNode.Options.Add(new SelectionOption(
                "기다려본다", "", true, null, EncounterSelectionOptions.WAIT));
            encounterNode.Options.Add(new SelectionOption(
                "말을 건다", "", true, null, EncounterSelectionOptions.TALK));
            encounterNode.Options.Add(SelectionOption.DynamicNode(
                "무언가를 던진다",
                "",
                hero.Inventory.ItemStacks.Count > 0,
                () => InventoryScreen.BuildThrowNode(hero, encounterNode)));
            encounterNode.Options.Add(new SelectionOption(
                "손을 뻗어 더듬는다", "", true, null, EncounterSelectionOptions.SEARCH));
            encounterNode.Options.Add(new SelectionOption(
                "전투 준비", "", true, battleNode, EncounterSelectionOptions.BATTLE));
            encounterNode.Options.Add(new SelectionOption(
                "취소", "", true, explorationNode, EncounterSelectionOptions.CANCEL));

            battleNode.Options.Add(new SelectionOption(
                "공격하기", "", true, null, AttackSelectionOptions.ATTACK));
            battleNode.Options.Add(new SelectionOption(
                "방어 자세", "", true, null, AttackSelectionOptions.DEFFENSE));
            battleNode.Options.Add(SelectionOption.DynamicNode(
                "스킬 선택",
                "사용할 스킬을 선택한다",
                hero.LearnedSkillIds.Count > 0,
                () => SkillScreen.BuildNode(
                    hero,
                    skillData,
                    battleNode)));
            battleNode.Options.Add(new SelectionOption(
                "취소", "", true, encounterNode, AttackSelectionOptions.CANCEL));

            moveNode.Options.Add(new SelectionOption(
                "돌아가기", "이전 선택지로 돌아간다",
                true,
                explorationNode));
            moveNode.Options.Add(new SelectionOption(
                "앞으로 간다", "",
                HasAvailableEdge(RoomDirection.FORWARD),
                null,
                MoveSelectionOptions.FORWARD));
            moveNode.Options.Add(new SelectionOption(
                "왼쪽으로 간다", "",
                HasAvailableEdge(RoomDirection.LEFT),
                null,
                MoveSelectionOptions.LEFT));
            moveNode.Options.Add(new SelectionOption(
                "오른쪽으로 간다", "",
                HasAvailableEdge(RoomDirection.RIGHT),
                null,
                MoveSelectionOptions.RIGHT));

            return new SelectionMenu(explorationNode);
        }

        public void OpenExplorationSelection()
        {
            if (explorationNode != null)
            {
                explorationPanel.OpenSelection(explorationNode);
            }
        }

        private bool HasAvailableEdge(RoomDirection direction)
        {
            if (currentRoom == null)
            {
                return false;
            }

            return currentRoom.Edges.ContainsKey(direction);
        }

        public void SetSlotState(int slotIndex, SlotState state)
        {
            if (currentRoom == null ||
                slotIndex < 0 || slotIndex >= currentRoom.Slots.Count)
            {
                return;
            }

            currentRoom.Slots[slotIndex].SetState(state);
            RefreshMoveOption();
            DrawSlots(View.Display, currentRoom.Slots);
            explorationPanel.DrawSelection();
        }

        public void RefreshRoom()
        {
            if (currentRoom == null)
            {
                return;
            }

            RefreshMoveOption();
            DrawSlots(View.Display, currentRoom.Slots);
            explorationPanel.DrawSelection();
        }

        private void RefreshMoveOption()
        {
            if (moveOption == null || currentRoom == null)
            {
                return;
            }

            moveOption.SetEnabled(CanMove(currentRoom.Slots));
        }

        private bool CanMove(List<RoomSlot> slots)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                RoomSlot slot = slots[i];
                if (slot.Type.HasDoor &&
                    slot.IsEmpty &&
                    slot.State == SlotState.REVEALED)
                {
                    return true;
                }
            }

            return false;
        }

        public int ChooseSlot()
        {
            if (currentRoom == null)
            {
                throw new InvalidOperationException(
                    "슬롯을 선택할 현재 방이 없습니다.");
            }

            return ChooseSlot(View.Display, currentRoom.Slots);
        }

        private int ChooseSlot(
            Viewport view,
            List<RoomSlot> slots)
        {
            int selected = 0;
            DrawSlots(view, slots);
            DrawSelector(view, selected);

            while (true)
            {
                ConsoleKey input = Utility.ReadInput();
                int previous = selected;

                if (input == ConsoleKey.LeftArrow)
                {
                    selected = (selected - 1 + SlotCount) % SlotCount;
                }
                else if (input == ConsoleKey.RightArrow)
                {
                    selected = (selected + 1) % SlotCount;
                }
                else if (input == ConsoleKey.Enter)
                {
                    view.ClearLine(Top + SlotHeight);
                    return selected;
                }

                if (previous != selected)
                {
                    DrawSelector(view, selected);
                }
            }
        }

        public void DrawSlots(
            Viewport view,
            List<RoomSlot> slots)
        {
            int left = (view.Width - GetTotalWidth()) / 2;

            view.Clear();
            for (int row = 0; row < SlotHeight; row++)
            {
                view.DrawLine(
                    Top + row,
                    BuildSlotLine(slots, row, left));
            }
        }

        private void DrawSelector(Viewport view, int selected)
        {
            int selectorRow = Top + SlotHeight;
            int left = (view.Width - GetTotalWidth()) / 2;
            int markerColumn = left + selected * (SlotWidth + SlotGap) +
                               SlotWidth / 2 - 1;

            view.ClearLine(selectorRow);
            view.DrawAt(selectorRow, markerColumn, "▲");
        }

        private string BuildSlotLine(
            List<RoomSlot> slots,
            int row,
            int left)
        {
            StringBuilder line = new StringBuilder(new string(' ', left));
            for (int i = 0; i < SlotCount; i++)
            {
                if (i > 0)
                {
                    line.Append(new string(' ', SlotGap));
                }
                line.Append(BuildSlot(slots[i], row));
            }

            return line.ToString();
        }

        private string BuildSlot(RoomSlot slot, int row)
        {
            SlotState slotState = slot.State;
            if (row == 0)
            {
                return slotState == SlotState.REVEALED
                    ? "┌" + new string('─', SlotWidth - 2) + "┐"
                    : "┌" + BuildHiddenBorder() + "┐";
            }

            if (row == SlotHeight - 1)
            {
                return slotState == SlotState.REVEALED
                    ? "└" + new string('─', SlotWidth - 2) + "┘"
                    : "└" + BuildHiddenBorder() + "┘";
            }

            string edge = "│";

            string contentName = "";

            if (slotState == SlotState.REVEALED)
            {
                if (slot.Content != null)
                {
                    contentName = slot.Content.Name;
                }
                else if (slot.Type.HasDoor)
                {
                    contentName = ExplorationMessages.Door();
                }
            }
            else if (slotState == SlotState.UNREVEALED &&
                     HasUnrevealedObject(slot))
            {
                contentName = "???";
            }

            if (row == SlotHeight / 2)
            {
                int contentWidth = Utility.GetDisplayWidth(contentName);
                int leftPadding = (SlotWidth - 2 - contentWidth) / 2;
                int rightPadding = SlotWidth - 2 - contentWidth - leftPadding;
                return edge + new string(' ', leftPadding) + contentName +
                       new string(' ', rightPadding) + edge;
            }

            if (!(slotState == SlotState.REVEALED))
            {
                return row % 2 == 1
                    ? edge + new string(' ', SlotWidth - 2) + edge
                    : new string(' ', SlotWidth);
            }

            return edge + new string(' ', SlotWidth - 2) + edge;
        }

        private bool HasUnrevealedObject(RoomSlot slot)
        {
            if (slot.Content != null)
            {
                return true;
            }

            return slot.Type.ObjectType ==
                       RoomObjectType.TreasureChest ||
                   slot.Type.ObjectType == RoomObjectType.Pile;
        }

        private string BuildHiddenBorder()
        {
            const string pattern = "─  ";
            StringBuilder border = new StringBuilder();
            while (border.Length < SlotWidth - 2)
            {
                border.Append(pattern);
            }

            return border.ToString(0, SlotWidth - 2);
        }

        private  int GetTotalWidth()
        {
            return SlotCount * SlotWidth + (SlotCount - 1) * SlotGap;
        }


    }
}
