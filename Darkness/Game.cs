using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darkness
{
    class Game
    {
        private GameManager gm;

        public Game()
        {
            gm = new GameManager();
        }

        void Config()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        void Initialize()
        {
            View.Initialize();
        }

        public void Start()
        {
            Config();
            Initialize();
            gm.Run();
        }

        
    }
}
