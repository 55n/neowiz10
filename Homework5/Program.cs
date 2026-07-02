using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework5
{
    class Program
    {
        static void Main(string[] args)
        {
            // 우리반 학생 검색기
            // 이름 입력 시 학생의 이름 | 성별 | 나이 | 휴대폰번호 | 나의 한줄평
            // 종료라고 입력하면 종료.

            Console.WriteLine("네오위즈 10기 학생 검색기. \'종료\'를 입력하면 종료됩니다.");
            while (true)
            {
                Console.Write("이름을 입력하세요: ");
                string input = Console.ReadLine();

                if (input == "종료") break;

                switch (input)
                {
                    case "김문규":
                        Console.WriteLine("이름: 김문규 | 성별: 남 | 나이: 29 | 전화번호: 010-8745-5847 | 한줄평: Don't starve");
                        break;
                    case "김종찬":
                        Console.WriteLine("이름: 김종찬 | 성별: 남  | 나이: 31 | 전화번호: 010-4114-4605 | 한줄평: 유니티");
                        break;
                    case "김주원":
                        Console.WriteLine("이름: 김주원 | 성별: 남  | 나이: 27 | 전화번호: 010-7697-9387 | 한줄평: 메이플");
                        break;
                    case "김태수":
                        Console.WriteLine("이름: 김태수 | 성별: 남 | 나이: 25 | 전화번호: 010-3927-7260 | 한줄평: 총명함");
                        break;
                    case "김표진":
                        Console.WriteLine("이름: 김표진 | 성별: 남 | 나이: 29 | 전화번호: 010-4911-6431 | 한줄평: 고수");
                        break;
                    case "김현민":
                        Console.WriteLine("이름: 김현민 | 성별: 남 | 나이: 27 | 전화번호: 010-7222-5892 | 한줄평: 운동 잘해서 부러움");
                        break;
                    case "김현호":
                        Console.WriteLine("이름: 김현호 | 성별: 남 | 나이: 33 | 전화번호: 010-6661-9228 | 한줄평: 집이 가까움");
                        break;
                    case "나영민":
                        Console.WriteLine("이름: 나영민 | 성별: 남 | 나이: 25 | 전화번호: 010-4622-8274 | 한줄평: 과묵하고 조용하신 분");
                        break;
                    case "박지은":
                        Console.WriteLine("이름: 박지은 | 성별: 여 | 나이: 25 | 전화번호: 010-9435-7730 | 한줄평: 새우");
                        break;
                    case "박진호":
                        Console.WriteLine("이름: 박진호 | 성별: 남 | 나이: 29 | 전화번호: 010-4112-8770 | 한줄평: 롤을 잘하시는 분");
                        break;
                    case "양성은":
                        Console.WriteLine("이름: 양성은 | 성별: 남 | 나이: 26 | 전화번호: 010-6396-7691 | 한줄평: 코딩 앤 기타");
                        break;
                    case "유현준":
                        Console.WriteLine("이름: 유현준 | 성별: 남 | 나이: 34 | 전화번호: 010-7378-1592 | 한줄평: 동갑");
                        break;
                    case "이건호":
                        Console.WriteLine("이름: 이건호 | 성별: 남 | 나이: 33 | 전화번호: 010-8814-4832 | 한줄평: 아침 식사를 거르지 않음");
                        break;
                    case "이도현":
                        Console.WriteLine("이름: 이도현 | 성별: 남 | 나이: 27 | 전화번호: 010-9789-5549 | 한줄평: 친절하시고 성실하시고 똑똑하시고 부지런하심");
                        break;
                    case "이소영":
                        Console.WriteLine("이름: 이소영 | 성별: 여 | 나이: 26 | 전화번호: 010-7180-5330 | 한줄평: 기획자");
                        break;
                    case "한덕현":
                        Console.WriteLine("이름: 한덕현 | 성별: 남 | 나이: 34 | 전화번호: 010-3906-0236 | 한줄평: 나");
                        break;
                    case "한은준":
                        Console.WriteLine("이름: 한은준 | 성별: 남 | 나이: 38 | 전화번호: 010-8768-2247 | 한줄평: 같은 한 씨 형");
                        break;
                    case "황영근":
                        Console.WriteLine("이름: 황영근 | 성별: 남 | 나이: 36 | 전화번호: 010-9241-5211 | 한줄평: 개발자");
                        break;
                    default:
                        Console.WriteLine("데이터에 없는 이름입니다.");
                        break;
                }
            }
        }
    }
}
