using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework14
{
    class GameMaster
    {
        public int stageNumber { get; private set; }
        public Stage stage { get; private set; }
        public Human player { get; private set; }
        public Dictionary<GameState, IGameState> GameStateDictionary { get; private set; }
        public GameState GameState;

        public GameMaster(Human player)
        {
            stageNumber = 0;
            this.player = player;
            GameStateDictionary = new Dictionary<GameState, IGameState>();
            GameStateDictionary.Add(GameState.PLAYING, new Playing(this));
            GameStateDictionary.Add(GameState.GAME_OVER, new GameOver(this));
            GameStateDictionary.Add(GameState.GAME_CLEAR, new GameClear(this));


        }

        public void createStage()
        {
            List<Creature> monsters = MonsterManager.Instance.AddMonsters(3);

            this.stageNumber++;
            this.stage = new Stage(monsters);
        }

        public Stage getStage() { return stage; }
        public void removeStage() { this.stage = null; }

        public void endStage()
        {
            int expReward = 0;

            for (int i = 0; i < this.stage.monsters.Count; i++)
            {
                expReward += this.stage.monsters[i].exp;
                Trade t = new Trade(this.player.inventory, this.stage.monsters[i].inventory);
                t.lootItems();
            }

            Console.WriteLine($"골드를 벌었습니다");
            player.inventory.veiwInventory();
            Console.WriteLine();
            Console.WriteLine($"스테이지 클리어 경험치를 받습니다: {expReward}exp");
            if (player.requiredExp <= expReward) player.levelUp(expReward - player.requiredExp);

            MonsterManager.Instance.ClearMonsters();
        }
    }

    class Stage
    {
        public List<Creature> monsters { get; set; }

        public Stage(List<Creature> monsters)
        {
            this.monsters = monsters;
        }

        public bool isStageClear()
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].creatureState == CreatureState.ALIVE) return false;
            }

            return true;
        }
    }
}
