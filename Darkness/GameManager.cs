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
        private Exploration _exploration;

        public PlayingState()
        {
        }

        public void Enter()
        {
            HeroType heroType = new HeroData().HeroTypes["hero"];
            ItemData itemData = new ItemData();
            ItemType guardianCharmType =
                itemData.ItemTypes["guardian_charm"];
            ItemType magicStoneType =
                itemData.ItemTypes["magic_stone"];
            EffectType guardianBlessingType =
                new EffectData().EffectTypes["guardian_blessing"];
            Inventory inventory = new Inventory(4);
            inventory.Store(new ItemStack(
                new Item(magicStoneType),
                2));
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
                inventory,
                equipment,
                new HashSet<string>
                {
                    "guardian_blessing",
                    "last_focus",
                    "power_strike",
                    "sword_last_day"
                },
                new List<ActiveEffect>
                {
                    new GuardianBlessingEffect(guardianBlessingType)
                });

            _exploration = new Exploration(hero, new Dungeon());
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
            Utility.PlayMessages(new[]
            {
                CombatMessages.PlayerDied(),
                CombatMessages.GameOver()
            });
        }

        public GameState? Action()
        {
            return GameState.INTRO;
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
