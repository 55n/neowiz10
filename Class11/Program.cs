using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class11
{
   

    class Program
    {
        static void Main(string[] args)
        {
            People p = new People("김태수");

            Console.WriteLine($"{p.Name}이 로또 1등에 당첨 되었습니다. 기분은 어떤가요?");
            p.State = State.Happy; // set을 사용해서 입력
            Console.WriteLine(p.State); // get을 사용하여 출력

            Console.WriteLine($"{p.Name}은 헌금으로 다 지출했습니다. 기분은 어떤가요?");
            p.State = State.Sad;
            Console.WriteLine(p.State); // get을 사용하여 출력

            Console.WriteLine($"{p.Name} 갑자기 당첨 용지를 뺏겼습니다. 기분은 어떤가요?");
            p.State = State.Angry;
            Console.WriteLine(p.State); // get을 사용하여 출력

            Console.WriteLine("==========================================================================");

            // State 패턴 - 상태에 따라 어떤 결과를 도출하거나 반환하는 형태.
            // 상태를 체크하여 행위를 호출하는 것이 아니라 낮은 결합도를 갖고 있음.
            Human h = new Human("성은양");
            
            Console.WriteLine($"{h.Name}이 오늘 선생님에게 숙제를 받지 않았습니다. 기분은 어떤가요?");
            h.stateDictionary[State.Happy].Action();
            
            Console.WriteLine($"{h.Name}은 오늘 목요일인 것을 몰랐습니다. 당신의 기분은 어떤가요?");
            h.stateDictionary[State.Angry].Action();

            Console.WriteLine($"{h.Name}은 알바를 나가서 물건이 왕창 들어왔습니다. 당신의 기분은?");
            h.stateDictionary[State.Sad].Action();
        }
    }
}
