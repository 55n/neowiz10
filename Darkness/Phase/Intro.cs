using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Darkness
{
    enum IntroSelectionOptions
    {
        START, END
    }
    class Intro
    {
        public GameSignal Run()
        {
            View.Display.Clear();
            View.Message.Clear();
            View.Display.DrawCenteredLineByLine(IntroImage.Lines);
            Utility.PlayMessages(new string[] { "[방향키와 엔터키로 진행합니다]" });
            Utility.PlayMessages(new string[] {
                "노련한 모험가인 당신은 여느 때와 같이 던전에 들어갔다",
                "온갖 몬스터와 함정들을 당신은 묵묵히 헤쳐 나갔다.",
                "그러던 중 운 나쁘게 발동된 발 밑의 함정. 꺼지는 바닥.",
                "그렇게 당신은"
            });

            string[] title = new string[] { "[어둠 속으로 떨어졌다]", "" };
            View.Message.DrawCentered(title);
            Thread.Sleep(700);

            SelectionOption titleOptions0 = new SelectionOption("게임 시작", "게임을 시작한다", true, null, IntroSelectionOptions.START);
            SelectionOption titleOptions1 = new SelectionOption("게임 종료", "게임을 종료한다", true, null, IntroSelectionOptions.END);
            List<SelectionOption> titleOptions = new List<SelectionOption>();
            titleOptions.Add(titleOptions0);
            titleOptions.Add(titleOptions1);
            SelectionNode titleNode = new SelectionNode("title_menu", "시작화면메뉴", titleOptions, null);
            SelectionMenu titleMenu = new SelectionMenu(titleNode);

            TitleMenuPanel titlePanel = new TitleMenuPanel(
                View.Message,
                titleMenu,
                3);

            IntroSelectionOptions selectedOptionValue = (IntroSelectionOptions)titlePanel.ReadSelection();

            if (selectedOptionValue == IntroSelectionOptions.START)
            {
                PlayFallingEffect(IntroImage.Lines.Length);
                return GameSignal.START_GAME;
            }
            else
            {
                Utility.PlayMessages(new string[] { "...라고 생각했지만 재빠르게 탈출했다.", "[끝]" });
                return GameSignal.EXIT_GAME;
            }
        }


        private void PlayFallingEffect(int imageHeight)
        {
            View.Message.Clear();

            for (int markerRow = 0; markerRow < imageHeight; markerRow++)
            {
                View.Message.DrawLineCentered(markerRow, ".");

                int imageRow = imageHeight - 1 - markerRow * 2;
                if (imageRow >= 0)
                {
                    View.Display.ClearLine(imageRow);
                    if (imageRow - 1 >= 0)
                    {
                        View.Display.ClearLine(imageRow - 1);
                    }
                }

                Thread.Sleep(100);
            }

            View.Message.DrawLineCentered(imageHeight, "쿵!!!");
            Thread.Sleep(1000);
        }
    }
}
