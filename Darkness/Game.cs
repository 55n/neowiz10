using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    class Game
    {
        void Config()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        void Initialize()
        {

        }

        public void GameLoop()
        {
            Config();
            new Ui.Narrative().DrawIntroImage();

            //View.Canvas.Draw(caveArt);
            string[] s = new string[]{
                "야",
                "야",
                "야"
            };

            View.Message.Draw(s);
        }
    }
}
