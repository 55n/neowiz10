using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * [예외처리]
 * if: 값으로 예외처리
 * try-catch: 상황으로 예외처리. 외부 프로그램과의 소통부에서 많이 사용, 감시에 자원을 소모함
 */

/*
 * [인덱스 기반 자료구조]
 * List: 크기가 바뀜! 자료형은 정해져 있음.
 * ArrayList: 데이터를 Object로 박싱(값 타입을 참조 타입으로 변경)하여 담고 데이터를 꺼낼 때는 언박싱 함 그래서 리소스를 더 먹음.
 */

namespace Class8
{
    class Student
    {
        public string name;
        public string age;

        public Student(string _name, string _age)
        {
            name = _name;
            age = _age;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // try-catch
            try
            {
                int[] arr = new int[5];
                for(int i = 0; i < 6; i++)
                {
                    arr[i] = i;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("오류가 발생");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("오류가 발생한 다음?");


            // 인덱스 기반 자료구조

            // Array
            string[] array = new string[5];
            for(int i = 0; i < array.Length; i++)
            {
                Console.Write("나라 이름: ");
                array[i] = Console.ReadLine();
            }

            for(int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }

            Console.WriteLine("------------------------------------------------");

            // List
            List<Student> youngStudent = new List<Student>();

            while (true)
            {
                Console.WriteLine("더 이상 없으면 \'끝\'이라고 입력");
                Console.Write("어린 친구들: ");
                string name = Console.ReadLine();

                if(name == "끝")
                {
                    break;
                }

                Console.Write($"{name} 몇 살?: ");
                string age = Console.ReadLine();

                youngStudent.Add(new Student(name, age));
            }

            Console.Clear();

            Console.Write("삭제할 친구: ");
            string deleteName = Console.ReadLine();

            for (int i = 0; i < youngStudent.Count; i++)
            {
                if(youngStudent[i].name == deleteName)
                {
                    youngStudent.RemoveAt(i);
                    // youngStudent.Remove(youngStudent[i]);
                    break;
                }
            }

            for(int i = 0; i < youngStudent.Count; i++)
            {
                Console.WriteLine($"친구 이름 {youngStudent[i].name} 나이: {youngStudent[i].age}");
            }

            youngStudent.Clear();

            Console.WriteLine(youngStudent.Count);

            for (int i = 0; i < youngStudent.Count; i++)
            {
                Console.WriteLine($"친구 이름 {youngStudent[i].name} 나이: {youngStudent[i].age}");
            }

            Console.WriteLine("------------------------------------------------");

            // ArrayList
            ArrayList oldStudents = new ArrayList();

            oldStudents.Add("박진호");
            oldStudents.Add(29.7f);
            oldStudents.Add("김문규");
            oldStudents.Add(29);
            oldStudents.Add("김표진");
            oldStudents.Add(28.9d);
            oldStudents.Add(new Student("황영근", "26"));
            oldStudents.Add('ㄱ');

            for(int i = 0; i < oldStudents.Count; i++)
            {
                Console.WriteLine($"oldStudent[{i}] => {oldStudents[i]} | type: {oldStudents[i].GetType()}");
            }
        }
    }
}
