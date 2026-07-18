using System;

namespace Darkness
{
    public class SelectionMenu
    {
        public SelectionNode CurrentNode { get; private set; }
        public int SelectedIndex { get; private set; }
        public SelectionOption SelectedOption { get; private set; }

        public SelectionMenu(SelectionNode rootNode)
        {
            Open(rootNode);
        }

        public void Open(SelectionNode node)
        {
            CurrentNode = node;
            SelectFirstEnabledOption();
        }

        public void MoveUp()
        {
            Move(-1);
        }

        public void MoveDown()
        {
            Move(1);
        }

        public void Confirm()
        {
            if (SelectedOption != null &&
                SelectedOption.Enabled &&
                SelectedOption.NextNode != null)
            {
                Open(SelectedOption.NextNode);
            }
        }

        public void Back()
        {
            if (CurrentNode != null && CurrentNode.Parent != null)
            {
                Open(CurrentNode.Parent);
            }
        }

        public void Clear()
        {
            CurrentNode = null;
            SelectedIndex = -1;
            SelectedOption = null;
        }

        public void Refresh()
        {
            if (SelectedOption == null || !SelectedOption.Enabled)
            {
                SelectFirstEnabledOption();
            }
        }

        private void SelectFirstEnabledOption()
        {
            SelectedIndex = -1;
            SelectedOption = null;

            if (CurrentNode == null || CurrentNode.Options == null)
            {
                return;
            }

            for (int i = 0; i < CurrentNode.Options.Count; i++)
            {
                if (CurrentNode.Options[i].Enabled)
                {
                    SelectedIndex = i;
                    SelectedOption = CurrentNode.Options[i];
                    return;
                }
            }
        }

        private void Move(int direction)
        {
            if (CurrentNode == null ||
                CurrentNode.Options == null ||
                CurrentNode.Options.Count == 0 ||
                SelectedIndex < 0)
            {
                return;
            }

            int candidate = SelectedIndex;
            for (int i = 0; i < CurrentNode.Options.Count; i++)
            {
                candidate = (candidate + direction + CurrentNode.Options.Count) %
                            CurrentNode.Options.Count;
                if (CurrentNode.Options[candidate].Enabled)
                {
                    SelectedIndex = candidate;
                    SelectedOption = CurrentNode.Options[candidate];
                    return;
                }
            }
        }
    }
}
