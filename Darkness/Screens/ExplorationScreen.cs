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
        ATTACK, DEFFENSE, SKILL, CANCEL
    }
    enum MoveSelectionOptions
    {
        BACK, FORWARD, LEFT, RIGHT
    }

    public class ExplorationScreen
    {
        private const int SlotCount = 5;
        private const int SlotWidth = 8;
        private const int SlotGap = 2;
        private const int SlotHeight = 7;
        private const int Top = 2;
        public MessagePanel explorationPanel { get; private set; }

        public void InitScreen(Room room)
        {
            explorationPanel = new MessagePanel(
                View.Message,
                BuildExplorationSelection(),
                0,
                2);

            explorationPanel.PlayNarrations(
                room.Type.EnterMessages);

            DrawSlots(View.Display, room.Type.Slots, );

            explorationPanel.OpenSelection(
                explorationPanel.SelectionMenu.CurrentNode);
        }

        public SelectionMenu BuildExplorationSelection()
        {
            SelectionNode explorationNode = new SelectionNode(
                "exploration-node",
                "",
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

            explorationNode.Options.Add(new SelectionOption(
                "상태창", "", true, null, ExplorationSelectionOptions.STATUS));
            explorationNode.Options.Add(new SelectionOption(
                "소지품", "", true, null, ExplorationSelectionOptions.INVENTORY));
            explorationNode.Options.Add(new SelectionOption(
                "행동", "", true, encounterNode, ExplorationSelectionOptions.ENCOUNTER));
            explorationNode.Options.Add(new SelectionOption(
                "이동", "", true, moveNode, ExplorationSelectionOptions.MOVE));
            explorationNode.Options.Add(new SelectionOption(
                "뒤로가기", "", true, null, ExplorationSelectionOptions.BACK));

            encounterNode.Options.Add(new SelectionOption(
                "기다려본다", "", true, null, EncounterSelectionOptions.WAIT));
            encounterNode.Options.Add(new SelectionOption(
                "말을 건다", "", true, null, EncounterSelectionOptions.TALK));
            encounterNode.Options.Add(new SelectionOption(
                "무언가를 던진다", "", true, null, EncounterSelectionOptions.THROW));
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
            battleNode.Options.Add(new SelectionOption(
                "스킬 선택", "", true, null, AttackSelectionOptions.SKILL));
            battleNode.Options.Add(new SelectionOption(
                "취소", "", true, encounterNode, AttackSelectionOptions.CANCEL));

            moveNode.Options.Add(new SelectionOption(
                "돌아간다", "돌아간다", true, null, MoveSelectionOptions.BACK));
            moveNode.Options.Add(new SelectionOption(
                "앞으로 간다", "", true, null, MoveSelectionOptions.FORWARD));
            moveNode.Options.Add(new SelectionOption(
                "왼쪽으로 간다", "", true, null, MoveSelectionOptions.LEFT));
            moveNode.Options.Add(new SelectionOption(
                "오른쪽으로 간다", "", true, null, MoveSelectionOptions.RIGHT));

            return new SelectionMenu(explorationNode);
        }

        public static int ChooseSlot(
            Viewport view,
            string[] slotContents,
            bool[] revealedSlots)
        {
            return ChooseSlot(view, slotContents, revealedSlots, true);
        }

        public static int ChooseAnySlot(
            Viewport view,
            string[] slotContents,
            bool[] revealedSlots)
        {
            return ChooseSlot(view, slotContents, revealedSlots, false);
        }

        private static int ChooseSlot(
            Viewport view,
            string[] slotContents,
            bool[] revealedSlots,
            bool skipRevealedSlots)
        {
            int selected = skipRevealedSlots
                ? Array.FindIndex(revealedSlots, revealed => !revealed)
                : 0;

            DrawSlots(view, slotContents, revealedSlots);
            DrawSelector(view, selected);

            while (true)
            {
                ConsoleKey input = Utility.ReadInput();
                int previous = selected;

                if (input == ConsoleKey.LeftArrow)
                {
                    do
                    {
                        selected = (selected - 1 + SlotCount) % SlotCount;
                    }
                    while (skipRevealedSlots && revealedSlots[selected]);
                }
                else if (input == ConsoleKey.RightArrow)
                {
                    do
                    {
                        selected = (selected + 1) % SlotCount;
                    }
                    while (skipRevealedSlots && revealedSlots[selected]);
                }
                else if (input == ConsoleKey.Enter)
                {
                    return selected;
                }

                if (previous != selected)
                {
                    DrawSelector(view, selected);
                }
            }
        }

        public static void DrawSlots(
            Viewport view,
            string[] slotContents,
            bool[] revealedSlots)
        {
            int left = (view.Width - GetTotalWidth()) / 2;

            view.Clear();
            for (int row = 0; row < SlotHeight; row++)
            {
                view.DrawLine(
                    Top + row,
                    BuildSlotLine(slotContents, revealedSlots, row, left));
            }
        }

        private static void DrawSelector(Viewport view, int selected)
        {
            int selectorRow = Top + SlotHeight;
            int left = (view.Width - GetTotalWidth()) / 2;
            int markerColumn = left + selected * (SlotWidth + SlotGap) + 3;

            view.ClearLine(selectorRow);
            view.DrawAt(selectorRow, markerColumn, "▲");
        }

        private static string BuildSlotLine(
            string[] slotContents,
            bool[] revealedSlots,
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
                line.Append(BuildSlot(slotContents[i], revealedSlots[i], row));
            }

            return line.ToString();
        }

        private static string BuildSlot(string content, bool revealed, int row)
        {
            if (row == 0)
            {
                return revealed ? "┌──────┐" : "┌─  ─  ┐";
            }

            if (row == SlotHeight - 1)
            {
                return revealed ? "└──────┘" : "└─  ─  ┘";
            }

            string edge = "│";
            if (!string.IsNullOrEmpty(content) && row == SlotHeight / 2)
            {
                int contentWidth = Utility.GetDisplayWidth(content);
                int leftPadding = (SlotWidth - 2 - contentWidth) / 2;
                int rightPadding = SlotWidth - 2 - contentWidth - leftPadding;
                return edge + new string(' ', leftPadding) + content +
                       new string(' ', rightPadding) + edge;
            }

            if (!revealed)
            {
                return row % 2 == 1 ? "│      │" : "        ";
            }

            return edge + new string(' ', SlotWidth - 2) + edge;
        }

        private static int GetTotalWidth()
        {
            return SlotCount * SlotWidth + (SlotCount - 1) * SlotGap;
        }
    }
}
