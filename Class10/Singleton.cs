using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * 싱글턴 패턴 - 오직 하나의 객체만 생성할 수 있는 클래스의 패턴
 * static과 다르게 선언 즉시 할당되지는 않고 instance를 호출하면 생성자가 호출되게 함(단 1회)
 * 
 */

namespace Class10
{
    class Singleton
    {
        private static Singleton _instance = null; // 빈 공간만 런타임 시작 시 전역 관리하도록 한다.
        
        public static Singleton Instance
        {
            get // 메서드 프로퍼티 형식
            {
                if(_instance == null) // 단 한번 생성하기 위해서 null인지 확인
                {
                    _instance = new Singleton();
                }

                return _instance;
            }
        }
        // ====================================================================================================

        public Singleton()
        {
            Console.WriteLine("안녕~싱글턴이다.");
        }

        public void Singleton_Method()
        {
            Console.WriteLine("싱글턴 메서드 호출됨!");
        }
    }

    class Test
    {
        public Test()
        {
            Console.WriteLine("Test class constructor called");
        }

        public void Test_Method()
        {
            Console.WriteLine("Test_Method 호출");
        }
    }
}
