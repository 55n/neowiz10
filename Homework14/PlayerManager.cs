using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Homework14
{
    class PlayerManager
    {
        private static PlayerManager _instance = null;

        public static PlayerManager Instance
        {
            get
            {
                if (_instance == null) _instance = new PlayerManager();
                return _instance;
            }
        }

        private int _playerChoice = 0;

        public void setPlayerChoice(int playerChoice)
        {
            _playerChoice = playerChoice;
        }

        public int getPlayerChoice()
        {
            return _playerChoice;
        }
    }
}
