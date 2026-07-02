using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class4
{
    class Program
    {
        // Enum - 컴파일 시 상수로 변환, 코딩 시 의미 있는 단어로 표현
        enum PlayerState
        {
            Attack,
            Move,
            Deffense
        }


        static void Main(string[] args)
        {
            PlayerState state;

            while(true)
            {
                break;

                Console.Write("용사여 당신은 무엇을 할 것인가: ");
                int stateNum = int.Parse(Console.ReadLine());
                
                state = (PlayerState)stateNum;

                switch (state)
                {
                    case PlayerState.Attack:
                        Console.WriteLine("용사는 공격했다!");
                        break;
                    case PlayerState.Move:
                        Console.WriteLine("용사는 움직였다!");
                        break;
                    case PlayerState.Deffense:
                        Console.WriteLine("용사는 방어했다!");
                        break;
                    default:
                        Console.WriteLine("잘못 입력됨!");
                        break;
                }
                Console.ReadLine();
                Console.Clear();

            }


            /* 
            * 형변환
            * 암시적 형변환: 코드에서 따로 작성하지 않아도 컴파일 시 자료형이 변함
            * 명시적 형변환: 코드에서 자료형의 변환을 명시하여 컴파일러에 전달
            */
            int a = 1;
            float b = a;

            Console.WriteLine(a);
            Console.WriteLine(b);

            float c = 1.5f;
            int d = (int)c;

            Console.WriteLine(c);
            Console.WriteLine(d);
            Console.WriteLine();

            // 배열
            /* 자료구조: 복수의 데이터를 담기 위해 사용하는 구조
             * 배열: 동일한 자료형, n개의 요소
             */
            int num1 = 1;
            int num2 = 2;
            int num3 = 3;
            int num4 = 4;
            int num5 = 5;
            int num6 = 6;

            int[] numArray = new int[5];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = i;
            }

            for(int i = 0; i < numArray.Length; i++)
            {
                Console.WriteLine($"numArray[{i}]={numArray[i]}");
            }
            Console.WriteLine();
            foreach (int num in numArray)
            {
                Console.WriteLine(num);
            }

            // 배열 초기화
            string[] nameArray = new string[10] { "김문규", "김종찬", "김주원", "김태수", "김표진", "김현호", "김현민", "나영민", "박지은", "박진호" };

            for(int i = 0; i < nameArray.Length; i++)
            {
                Console.WriteLine($"namearray[{i}] = {nameArray[i]}");
            }

            for(int i = 0; i < nameArray.Length; i++)
            {
                break;
                Console.Write($"{i}번째 이름을 입력해주세요: ");
                string name = Console.ReadLine();
                nameArray[i] = name;
            }

            
            char[] charArray = new char[10];

            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] = (char)(i + 65);
            }

            for (int i = 0; i < charArray.Length; i++)
            {
                Console.WriteLine(charArray[i]);
            }

            // 순서를 보장하지 않아도 되는 탐색에 사용
            foreach (char i in charArray)
            {
                Console.WriteLine(i);
            }

            // shuffle 알고리즘
            Console.WriteLine("셔플");
            char tmpChar;

            Random random = new Random();

            for(int i = 0; i < 1000; i++)
            {
                int destIndex = random.Next() % charArray.Length;
                int sourIndex = random.Next() % charArray.Length;
                tmpChar = charArray[destIndex];
                charArray[destIndex] = charArray[sourIndex];
                charArray[sourIndex] = tmpChar;
            }

            for(int i = 0; i < charArray.Length; i++)
            {
                Console.WriteLine(charArray[i]);
            }

            Console.WriteLine("로또번호생성기");
            int[] arr = new int[45];

            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = i + 1;
            }

            int tmpNum;
            Random rnd = new Random();

            for(int i = 0; i < 1000; i++)
            {
                int destIndex = (rnd.Next()) % arr.Length;    
                int sourIndex = (rnd.Next()) % arr.Length;
                tmpNum = arr[destIndex];
                arr[destIndex] = arr[sourIndex];
                arr[sourIndex] = tmpNum;
            }

            for(int i = 0; i < 6; i++)
            {
                Console.WriteLine(arr[i]);
            }
        }
    }
}
