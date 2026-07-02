using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * 1. updown 게임을 반복문으로 변경
             * 2. 가위바위보 - 유저 베팅, 컴퓨터도 같은 금액 베팅. 소지금 동일. 유저 입력, 컴퓨터 입력. 비기면 둘 다 잃음. 둘중 하나가 텅 비면 게임 종료.
             */
            Console.WriteLine("업다운 게임을 시작합니다!!");

            // 정답 생성
            Random randomObj = new Random();
            int randomValue = randomObj.Next();
            int answer = randomValue % 1000;

            // 플레이어 입력
            int input;

            // 생명 인디케이터
            int life = 10;

            // 생명이 0이 되거나 정답을 맞출 때까지 반복
            while(life >= 0)
            {
                if(life==0)
                {
                    Console.WriteLine($"정답은 {answer} 입니다~ㅋㅋ");
                    break;
                }

                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                    break;
                }

                life--;
            }
        }
    }
}
