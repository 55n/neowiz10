using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Homework9
{
    class GameMaster
    {
        public int stageNumber { get; private set; }
        public Stage stage { get; private set; }
        public Human player { get; private set; }

        public GameMaster(Human player)
        {
            stageNumber = 0;
            this.player = player;
        }

        public void createStage()
        {
            Random random = new Random();

            int monsterNumber = random.Next() % 3 + 1;

            Creature[] monsters = new Creature[monsterNumber];

            for (int i = 0; i < monsters.Length; i++)
            {
                int species = random.Next() % 3;
                if(species == 0) monsters[i] = new Slug();
                else if(species == 1) monsters[i] = new Slime();
                else if(species == 2) monsters[i] = new MushMom();
            }

            this.stageNumber++;
            this.stage = new Stage(monsters);
        }

        public Stage getStage() { return stage; }


        public void endStage()
        {
            int expReward = 0;

            for (int i = 0; i < this.stage.monsters.Length; i++)
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
        }

    }

    class Stage
    {
        public Creature[] monsters { get; set; }

        public Stage(Creature[] monsters)
        {
            this.monsters = monsters;
        }

        public bool isStageClear()
        {
            for (int i = 0; i < this.monsters.Length; i++)
            {
                if(this.monsters[i].creatureState == CreatureState.LIVING) return false;
            }
            
            return true;
        }
    }
}
