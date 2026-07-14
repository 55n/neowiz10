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
            //new Ui.Narrative().DrawIntroImage();
            //Utility.WriteLine("야");
            //Utility.WriteLine("야");
            //Utility.WriteLine("야");

            //View.Canvas.Draw(caveArt);
            string[] s = new string[]{
                "어둠 속으로 떨어졌다.",
                "",
                "[Enter]"
            };

            View.Message.Draw(s);
        }
    }
}
