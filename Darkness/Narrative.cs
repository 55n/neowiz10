using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness.Ui
{
    class Narrative
    {
        public void DrawIntroImage()
        {
            string[] cave =
            {
                "                  ▄▄████████████▄▄                  ",
                "               ▄██████▓▓▓▓▓▓▓▓▓██████▄              ",
                "             ▄████▓▓▓▒▒▒▒▒▒▒▓▓████▄            ",
                "           ▄███▓▓▒▒░           ░░▒▒▓▓███▄          ",
                "          ███▓▒▒░░   ▄▄██████▄▄   ░░▒▒▓███          ",
                "        ▄██▓▒░░   ▄██████████████▄   ░░▒▓██▄        ",
                "       ▄██▓▒░   ▄██████████████████▄   ░▒▓██▄       ",
                "      ███▓▒░   █████▀▀          ▀▀█████   ░▒▓███      ",
                "     ███▓▒░   ████▀                ▀████   ░▒▓███     ",
                "    ███▓▒░   ████                    ████   ░▒▓███    ",
            };

            int artWidth = cave.Max(x => x.Length);
            int artHeight = cave.Length;

            int left = Math.Max(0, (Console.WindowWidth - artWidth) / 2);
            //int top = Math.Max(0, (Console.WindowHeight - artHeight) / 2);

            // 위쪽 가운데
            Console.SetCursorPosition(left, 0);
            Console.WriteLine();
            foreach (string line in cave)
            {
                Console.SetCursorPosition(left, Console.CursorTop);
                Console.WriteLine(line);
            }
        }

        public string[] IntroNarration()
        {
            string[] script = {
                "평범한 모험가인 당신은 연 때와 같이 던전에 들어갔다",
                "온갖 몬스터와 함정들을 당신은 묵묵히 헤쳐 나갔다.",
                "그러던 중 운 나쁘게 발동된 발 밑의 함정. 꺼지는 바닥.",
                "그렇게 당신은"
            };
            return script;
        }
    }
}
