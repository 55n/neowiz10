using System;
using System.Collections.Generic;

namespace Darkness
{
    public class MessagePanel
    {
        public Viewport View { get; private set; }
        public Narration Narration { get { return Darkness.Narration.Instance; } }
        public SelectionMenu SelectionMenu { get; private set; }
        public int NarrationStartRow { get; private set; }
        public int SelectionStartRow { get; private set; }

        public MessagePanel(
            Viewport view,
            SelectionMenu selectionMenu,
            int narrationStartRow,
            int selectionStartRow)
        {
            View = view;
            SelectionMenu = selectionMenu;
            NarrationStartRow = narrationStartRow;
            SelectionStartRow = selectionStartRow;
        }

        public void PlayNarrations(IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                SetNarration(message);
                Utility.WaitForEnter();
            }
        }

        public void SetNarration(string text)
        {
            Narration.SetLine(text);
            DrawNarration();
        }

        public void SetNarration(string[] lines)
        {
            Narration.SetLines(lines);
            DrawNarration();
        }

        public void AppendNarration(string[] lines)
        {
            Narration.AppendLines(lines);
            DrawNarration();
        }

        public void OpenSelection(SelectionNode node)
        {
            SelectionMenu.Open(node);
            UpdateSelectionNarration();
            DrawSelection();
        }

        public void CloseSelection()
        {
            ClearSelection();
        }

        public void MoveSelectionUp()
        {
            SelectionMenu.MoveUp();
            UpdateSelectionNarration();
            DrawSelection();
        }

        public void MoveSelectionDown()
        {
            SelectionMenu.MoveDown();
            UpdateSelectionNarration();
            DrawSelection();
        }

        public object ConfirmSelection()
        {
            object value = SelectionMenu.SelectedOption == null
                ? null
                : SelectionMenu.SelectedOption.Value;
            SelectionMenu.Confirm();
            UpdateSelectionNarration();
            DrawSelection();
            return value;
        }

        public object ReadSelection()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow)
                {
                    MoveSelectionUp();
                }
                else if (key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
                {
                    MoveSelectionDown();
                }
                else if (key == ConsoleKey.Enter)
                {
                    object value = ConfirmSelection();
                    if (value != null)
                    {
                        return value;
                    }
                }
            }
        }

        public void ClearNarration()
        {
            Narration.Clear();
            for (int row = NarrationStartRow; row < SelectionStartRow; row++)
            {
                View.ClearLine(row);
            }
        }

        public void ClearSelection()
        {
            SelectionMenu.Clear();
            for (int row = SelectionStartRow; row < View.Height; row++)
            {
                View.ClearLine(row);
            }
        }

        public void DrawNarration()
        {
            for (int row = NarrationStartRow; row < SelectionStartRow; row++)
            {
                View.ClearLine(row);
            }

            int availableRows = SelectionStartRow - NarrationStartRow;
            for (int i = 0; i < Narration.Lines.Count && i < availableRows; i++)
            {
                View.DrawLine(NarrationStartRow + i, Narration.Lines[i]);
            }
        }

        public void DrawSelection()
        {
            SelectionMenu.Refresh();

            for (int row = SelectionStartRow; row < View.Height; row++)
            {
                View.ClearLine(row);
            }

            if (SelectionMenu.CurrentNode == null)
            {
                return;
            }

            ConsoleColor previousColor = Console.ForegroundColor;
            for (int i = 0;
                 i < SelectionMenu.CurrentNode.Options.Count && SelectionStartRow + i < View.Height;
                 i++)
            {
                SelectionOption option = SelectionMenu.CurrentNode.Options[i];
                Console.ForegroundColor = option.Enabled
                    ? previousColor
                    : ConsoleColor.DarkGray;
                string cursor = i == SelectionMenu.SelectedIndex ? "> " : "  ";
                View.DrawLine(SelectionStartRow + i, cursor + option.Text);
            }

            Console.ForegroundColor = previousColor;
        }

        private void UpdateSelectionNarration()
        {
            if (SelectionMenu.CurrentNode == null)
            {
                return;
            }

            if (SelectionMenu.SelectedOption == null)
            {
                Narration.SetLine(SelectionMenu.CurrentNode.Description);
            }
            else
            {
                Narration.SetLines(new[]
                {
                    SelectionMenu.CurrentNode.Description,
                    SelectionMenu.SelectedOption.Description
                });
            }

            DrawNarration();
        }
    }
}
