using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("업다운 게임을 시작합니다!!");

            // 정답 생성
            Random randomObj = new Random();
            int randomValue = randomObj.Next();
            int answer = randomValue % 1000;

            // 플레이어 입력
            int input;

            // 생명 인디케이터
            int life = 10;

            // 1
            if (life == 10)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //2
            if (life == 9)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //3
            if (life == 8)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //4
            if (life == 7)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //5
            if (life == 6)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //6
            if (life == 5)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //7
            if (life == 4)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //8
            if (life == 3)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //9
            if (life == 2)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            //10
            if (life == 1)
            {
                Console.Write($"1부터 999 사이의 숫자를 입력하세요(남은 생명: {life}): ");
                input = int.Parse(Console.ReadLine());
                if (answer > input)
                {
                    Console.WriteLine("Up!");
                    life--;
                }
                else if (answer < input)
                {
                    Console.WriteLine("Down!");
                    life--;
                }
                else
                {
                    Console.WriteLine("정답입니다~!");
                }
            }
            // 게임오버
            if(life == 0)
            {
                Console.WriteLine($"정답은 {answer} 입니다! ㅋㅋㅋㅋ");
            }
        }
    }
}
