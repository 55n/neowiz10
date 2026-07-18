using System;
using System.Collections.Generic;

namespace Darkness
{
    class GameManager
    {
        private Dictionary<GameState, IGameState> _gameStateDictionary;
        private GameState _gameState;

        public GameManager()
        {
            GamePhase gamePhase = new GamePhase();

            _gameStateDictionary = new Dictionary<GameState, IGameState>();
            _gameStateDictionary.Add(GameState.INTRO, new IntroState(gamePhase));
            _gameStateDictionary.Add(GameState.PLAYING, new PlayingState(gamePhase));
            _gameStateDictionary.Add(GameState.STOP, new StopState());
        }

        private void ChangeState(GameState gameState)
        {
            _gameStateDictionary[_gameState].Exit();
            _gameState = gameState;
            _gameStateDictionary[_gameState].Enter();
        }

        public void Run()
        {
            _gameState = GameState.INTRO;

            _gameStateDictionary[_gameState].Enter();

            while (_gameState != GameState.STOP)
            {
                GameState? nextState = _gameStateDictionary[_gameState].Action();
                if (nextState.HasValue)
                {
                    ChangeState(nextState.Value);
                }
            }
        }
    }

    public enum GameSignal
    {
        START_GAME, EXIT_GAME
    }

    enum GameState
    {
        INTRO,
        PLAYING,
        STOP
    }

    interface IGameState
    {
        void Enter();
        GameState? Action();
        void Exit();
    }

    class IntroState : IGameState
    {
        public void Enter()
        {
        }
        public GameState? Action()
        {
            GameSignal gameSignal = new Intro().Run();
            if (gameSignal == GameSignal.START_GAME)
            {
                return GameState.PLAYING;
            }

            return GameState.STOP;
        }
        public void Exit()
        {
        }
    }

    class PlayingState : IGameState
    {
        public void Enter()
        {
        }
        public GameState? Action()
        {
            /*GameSignal gameSignal = _gamePhase.Exploration();
            if (gameSignal == GameSignal.EXIT_GAME)
            {
                return GameState.STOP;
            }*/

            return null;
        }
        public void Exit()
        {
        }
    }

    class StopState : IGameState
    {
        public void Enter()
        {
        }
        public GameState? Action()
        {
            return null;
        }
        public void Exit()
        {
        }
    }
}
