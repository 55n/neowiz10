using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("증감연산자");
            // 증감연산자 ++(증가), --(감소)
            int num = 10;
            Console.WriteLine(num);
            num--;
            Console.WriteLine(num);
            // 전위연산 vs 후위연산
            int num2 = 10;
            Console.WriteLine(++num2);
            Console.WriteLine(num2++);
            Console.WriteLine(num2);
            Console.WriteLine("===================================================================================");

            // 관계연산자 - return bool
            Console.WriteLine("관계연산자");
            int num3 = 10;
            int num4 = 20;

            bool result1 = num3 < num4;
            bool result2 = num3 > num4;
            bool result3 = num3 <= num4;
            bool result4 = num3 >= num4;
            bool result5 = num3 == num4;
            bool result6 = num3 != num4;

            Console.WriteLine($"1번 결과: {result1}");
            Console.WriteLine($"2번 결과: {result2}");
            Console.WriteLine($"3번 결과: {result3}");
            Console.WriteLine($"4번 결과: {result4}");
            Console.WriteLine($"5번 결과: {result5}");
            Console.WriteLine($"6번 결과: {result6}");
            Console.WriteLine("===================================================================================");

            // 사칙연산 - Tip. 나눗셈보다 곱셈이 빠르다 8 / 2 보다는 8 * 0.5가 빠름
            Console.WriteLine("사칙연산");
            int num5 = 8;

            Console.WriteLine(num5 + 2);
            Console.WriteLine(num5 - 2);
            Console.WriteLine(num5 * 2);
            Console.WriteLine(num5 / 3);
            Console.WriteLine(num5 % 3);
            Console.WriteLine("===================================================================================");

            // 논리연산자
            Console.WriteLine("논리연산자");
            int num6 = 10;
            int num7 = 20;

            bool res1 = (num6 == 9 && num7 == 20);
            bool res2 = (num6 <= 10 || num7 < 20);

            Console.WriteLine(res1);
            Console.WriteLine(res2);

            Console.WriteLine("===================================================================================");
            // 비트연산자
            Console.WriteLine("비트연산자");
            int bitNum1 = 15; // 0000 1111
            int bitNum2 = 20; // 0001 0100

            Console.WriteLine(bitNum1 & bitNum2); // 0000 0100
            Console.WriteLine(bitNum1 | bitNum2); // 0001 1111
            Console.WriteLine("===================================================================================");

            // 시프트 연산자
            Console.WriteLine("시프트 연산자");
            int bitNum3 = 15; // 0000 1111

            int shiftResult1 = bitNum3 << 1; // 0001 1110
            int shiftResult2 = bitNum3 << 2; // 0011 1100
            int shiftResult3 = bitNum3 << 3; // 0111 1000
            int shiftResult4 = bitNum3 << 4; // 1111 0000

            Console.WriteLine(shiftResult1);
            Console.WriteLine(shiftResult2);
            Console.WriteLine(shiftResult3);
            Console.WriteLine(shiftResult4);

            int shiftResult5 = bitNum3 >> 1; // 0000 0111
            int shiftResult6 = bitNum3 >> 2; // 0000 0011
            int shiftResult7 = bitNum3 >> 3; // 0000 0001
            int shiftResult8 = bitNum3 >> 4; // 0000 0000

            Console.WriteLine(shiftResult5);
            Console.WriteLine(shiftResult6);
            Console.WriteLine(shiftResult7);
            Console.WriteLine(shiftResult8);
            Console.WriteLine("===================================================================================");

            // 할당연산자(복합 대입연산자)
            Console.WriteLine("할당연산자(복합 대입연산자)");
            int playerHP = 20;
            int potion = 5;

            Console.WriteLine("플레이어 체력: " + playerHP);
            Console.WriteLine("플레이어가 포션을 섭취함");

            playerHP += potion;
            
            Console.WriteLine("플레이어 체력: " + playerHP);
            Console.WriteLine("===================================================================================");


            // 조건문 - 코드의 흐름을 제어하는 구문
            Console.WriteLine("조건문");
            /* 
             * [예외처리란]
             * 특정한 조건에 의해 어떤 무언가를 실행하는 것. 2가지 종류로 나뉨.
             * 1. 특정 값에 의한 예외처리
             * 2. 특정 상황에 의한 예외처리 ex) 네트워크 통신 실패
             * 1번에는 if를 사용
             * 2번에는 try-catch를 사용
             * 
             */
            int num10 = 15;

            if(num10 >= 12)
            {
                Console.WriteLine("오? 10보다 크거나 같음");

                if(num10 >= 15)
                {
                    Console.WriteLine("15보다 크거나 같은데?");
                } 
                else
                {
                    Console.WriteLine("15보다 작은데?");
                }
            }
            else if(num10 >= 10)
            {
                Console.WriteLine("10보다 크거나 같다");
            }

            Console.WriteLine($"num은 {num10}이야");

            Console.WriteLine("===================================================================================");

            bool isActive = false;
            
            if(isActive)
            {
                Console.WriteLine("활성화");
            }
            if (!isActive)
            {
                Console.WriteLine("비활성화");
            } // 하나의 조건문을 두개로 나누지 말 것

            if (isActive)
            {
                Console.WriteLine("활성화");
            }
            else if (!isActive)
            {
                Console.WriteLine("비활성화");
            }

            if (isActive)
            {
                Console.WriteLine("활성화");
            }
            else
            {
                Console.WriteLine("비활성화");
            }

            Console.WriteLine("===================================================================================");
            // 논리곱은 논리합보다 우선한다

            int num11 = 10;
            int num12 = 20;
            int num13 = 30;

            if((num11 > 5) || (num12 > 20) && (num13==30))
            {
                Console.WriteLine("실행~");
            }

            Console.WriteLine("===================================================================================");
            Console.WriteLine("문제1");

            int number1 = 10;
            int number2 = 20;
            int number3 = 30;

            if(number1 > 5 && number1 < 15)
            {
                if(number2 > number1)
                {
                    Console.WriteLine(number2);
                }else
                {
                    Console.WriteLine(number1);
                }
            }else
            {
                if(number3 > number1)
                {
                    Console.WriteLine(number1);
                }
                else
                {
                    Console.WriteLine(number3);
                }
            }

            Console.WriteLine("===================================================================================");
            Console.WriteLine("문제2");
            Console.WriteLine();
            Console.Write("본인의 소주의 주량을 입력하세요: ");
            int num_soju = int.Parse(Console.ReadLine());

            if(num_soju > 0)
            {
                if(num_soju%2 == 0)
                {
                    Console.WriteLine("짝수 주량 ㅋㅋㅋ");
                }
                else
                {
                    Console.WriteLine("홀수 주량");
                }
            }
            else
            {
                Console.WriteLine("심심한 인생...");
            }
        }
    }
}
