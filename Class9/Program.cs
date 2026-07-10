using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class9
{
    class Program
    {
        static void Main(string[] args)
        {
            // 스택
            Console.WriteLine("[스택]");
            Stack<int> stack = new Stack<int>();

            for(int i = 0; i < 10; i++)
            {
                stack.Push(i);
            }

            int count = stack.Count;
            for(int i = 0; i < count; i++)
            {
                Console.WriteLine(stack.Pop());
            }


            Console.WriteLine("===============================================");

            // 큐
            Console.WriteLine("[큐]");
            Queue<int> queue = new Queue<int>();

            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
            }

            int queueCount = queue.Count;

            for(int i = 0; i < queueCount; i++)
            {
                Console.WriteLine(queue.Dequeue());
            }

            Console.WriteLine("===============================================");

            // 딕셔너리
            Console.WriteLine("[딕셔너리]");
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("양성은", "양양이");
            dictionary.Add("김종찬", "Null");
            dictionary.Add("나영민", "대식이");
            dictionary.Add("김현호", "교주님");
            dictionary.Add("김현민", "너클");
            dictionary.Add("이도현", "영남");
            dictionary.Add("김태수", "교회오빠");
            dictionary.Add("김진호", "공주");
            dictionary.Add("이건호", "공주");
            dictionary.Add("박지은", "김첨지의 럭키데이");
            //dictionary.Add("김태수", "전봇대");

            foreach(KeyValuePair<string, string> p in dictionary)
            {
                Console.WriteLine($"k:{p.Key} | v:{p.Value}");
            }

            while (true)
            while (false)
            {
                Console.Write("별명을 검색할 친구의 이름: ");

                string keyName = Console.ReadLine();

                if (dictionary.ContainsKey(keyName))
                {
                    Console.WriteLine($"{keyName} 의 별명은 [{dictionary[keyName]}] 입니다.");
                }
                else
                {
                    Console.WriteLine($"{keyName}의 별명을 찾을 수 없습니다.");
                }
            }

            Console.WriteLine("=======================================================");

            // 해시테이블
            Console.WriteLine("[해시테이블]");

            Hashtable  hashtable = new Hashtable();

            hashtable.Add(1, "김문규");
            hashtable.Add("김문규", 1);
            hashtable.Add('ㄱ', "김");
            hashtable.Add(1.1f, 1.1d);
            hashtable.Add(153, "zzz");

            if (hashtable.ContainsKey("김문규"))
            {
                Console.WriteLine(hashtable["김문규"]);
            }
        }

    }
}
