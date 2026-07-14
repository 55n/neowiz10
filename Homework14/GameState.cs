using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    enum GameState
    {
        PLAYING,
        GAME_OVER,
        GAME_CLEAR
    }
    
    interface IGameState
    {
        void Action();
    }

    class Playing : IGameState
    {
        private GameMaster _gm;

        public Playing(GameMaster gm)
        {
            _gm = gm;
        }

        public void Action()
        {
            _gm.GameState = GameState.PLAYING;
            Console.WriteLine("게임 중...");
        }
    }
    class GameOver : IGameState
    {
        private GameMaster _gm;

        public GameOver(GameMaster gm)
        {
            _gm = gm;
        }
        public void Action()
        {
            _gm.GameState = GameState.GAME_OVER;
            Console.WriteLine("게임 오버");
        }
    }
    class GameClear : IGameState
    {
        private GameMaster _gm;

        public GameClear(GameMaster gm)
        {
            _gm = gm;
        }
        public void Action()
        {
            _gm.GameState = GameState.GAME_CLEAR;
            Console.WriteLine("게임 클리어");
        }
    }
}
