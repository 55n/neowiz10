using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class10
{
    class Program
    {
        public void A()
        {
            Test test = new Test();
            test.Test_Method();

            Singleton.Instance.Singleton_Method();
        }

        public void B()
        {

            Test test = new Test();
            test.Test_Method();

            Singleton.Instance.Singleton_Method();
        }


        static void MadeChoco(ChocolateFactory factory)
        {
            factory.MakeChocolate();
        }

        static void Main(string[] args)
        {
            /*Test test = new Test();
            test.Test_Method();

            Singleton.Instance.Singleton_Method();

            Program p = new Program();

            p.A();
            p.B();*/

            /*MonsterManager.Instance.Create_Monster("성은양", 4);
            MonsterManager.Instance.Create_Monster("소영이", 25);
            MonsterManager.Instance.Create_Monster("종찬킴", 3);
            MonsterManager.Instance.Create_Monster("건호2", 50);
            MonsterManager.Instance.Create_Monster("현준_너어~", 100000);
            MonsterManager.Instance.Create_Monster("덕풍동빨간장갑", 500000);

            MonsterManager.Instance.All_Viewing_Monster();*/


            ChocolateFactory milkChocolate = new MilkChocolateFactory();
            ChocolateFactory darkChocolate = new DarkChocolateFactory();
            ChocolateFactory whiteChocolate = new WhiteChocolateFactory();


            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Q) MadeChoco(milkChocolate);
                else if (keyInfo.Key == ConsoleKey.W) MadeChoco(whiteChocolate);
                else if (keyInfo.Key == ConsoleKey.E) MadeChoco(darkChocolate);
                else Console.WriteLine("꺼지셈 ㅋㅋ");
            }
        }
    }
}
