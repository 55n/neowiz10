using System;

namespace Darkness
{
    public class TitleMenuPanel
    {
        private readonly Viewport view;
        private readonly SelectionMenu menu;
        private readonly int startRow;

        public TitleMenuPanel(
            Viewport view,
            SelectionMenu menu,
            int startRow)
        {
            this.view = view;
            this.menu = menu;
            this.startRow = startRow;
        }

        public object ReadSelection()
        {
            Draw();

            while (true)
            {
                ConsoleKey key = Utility.ReadInput();
                if (key == ConsoleKey.UpArrow || key == ConsoleKey.LeftArrow)
                {
                    menu.MoveUp();
                    Draw();
                }
                else if (key == ConsoleKey.DownArrow || key == ConsoleKey.RightArrow)
                {
                    menu.MoveDown();
                    Draw();
                }
                else if (key == ConsoleKey.Enter && menu.SelectedOption != null)
                {
                    return menu.SelectedOption.Value;
                }
            }
        }

        private void Draw()
        {
            ConsoleColor previousColor = Console.ForegroundColor;

            for (int i = 0; i < menu.CurrentNode.Options.Count; i++)
            {
                SelectionOption option = menu.CurrentNode.Options[i];
                Console.ForegroundColor = option.Enabled
                    ? previousColor
                    : ConsoleColor.DarkGray;

                string cursor = i == menu.SelectedIndex ? "> " : "  ";
                view.DrawLineCentered(startRow + i, cursor + option.Text);
            }

            Console.ForegroundColor = previousColor;
        }
    }
}
