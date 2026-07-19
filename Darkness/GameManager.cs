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
            _gameStateDictionary = new Dictionary<GameState, IGameState>();
            _gameStateDictionary.Add(GameState.INTRO, new IntroState());
            _gameStateDictionary.Add(GameState.PLAYING, new PlayingState());
            _gameStateDictionary.Add(GameState.GAME_OVER, new GameOverState());
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
        START_GAME, GAME_OVER, EXIT_GAME
    }

    enum GameState
    {
        INTRO,
        PLAYING,
        GAME_OVER,
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
        private readonly Exploration _exploration;

        public PlayingState()
        {
            HeroType heroType = new HeroData().HeroTypes["hero"];
            ItemType guardianCharmType = new ItemData().ItemTypes["guardian_charm"];
            EffectType guardianBlessingType =
                new EffectData().EffectTypes["guardian_blessing"];
            Dictionary<EquipmentSlot, ItemStack> equipment =
                new Dictionary<EquipmentSlot, ItemStack>
                {
                    {
                        EquipmentSlot.Accessory,
                        new ItemStack(new Item(guardianCharmType), 1)
                    }
                };
            Hero hero = new Hero(
                heroType,
                new Inventory(4),
                equipment,
                new HashSet<string>(),
                new List<ActiveEffect>
                {
                    new GuardianBlessingEffect(guardianBlessingType)
                });

            _exploration = new Exploration(hero, new Dungeon());
        }

        public void Enter()
        {
        }
        public GameState? Action()
        {
            GameSignal gameSignal = _exploration.Run();
            if (gameSignal == GameSignal.GAME_OVER)
            {
                return GameState.GAME_OVER;
            }

            if (gameSignal == GameSignal.EXIT_GAME)
            {
                return GameState.STOP;
            }

            return null;
        }
        public void Exit()
        {
        }
    }

    class GameOverState : IGameState
    {
        public void Enter()
        {
            Utility.PlayMessage(CombatMessages.PlayerDied());
        }

        public GameState? Action()
        {
            return GameState.STOP;
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
