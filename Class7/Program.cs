using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class7
{
    class Parent
    {
        public string name;
        public int age;

        protected int money;

        public Parent()
        {
            this.money = 10000;
        }

        public void viewMoney()
        {
            Console.WriteLine("내 자산은 {0}", this.money);
        }

        public void scream(string name)
        {
            Console.WriteLine($"{name} 야!!!!!!!!!!!!!");
        }
    }

    class Children : Parent
    {
        public Children()
        { 
            // 하위 클래스가 멤버를 따로 만들지 않는 경우
            // this.money 는 부모의 멤버를 참조하고 있다
            // 생각해보니 당연한 말인데??
            this.money = 1000;
        }

        public void run()
        {
            base.money -= 1000;
            base.viewMoney();
            Console.WriteLine("{0} 튑니다~", this.name);
        }
    }

    class Program
    {
        public int sum(int a, int b)
        {
            int result = a + b;
            return result;
        }

        public float sum(float a, float b)
        {
            float result = a + b;
            return result;
        }

        public int sum(int a, int b, int c)
        {
            int result = a + b + c;
            return result;
        }

        static void Main(string[] args)
        {
            Parent parent = new Parent();
            parent.name = "한은준";
            parent.age = 38;

            Children children = new Children();
            children.name = "한영민";
            children.age = 25;

            parent.viewMoney();
            parent.scream("양성은");

            children.viewMoney();
            children.scream("양성은");
            children.run();

            Dog cancho = new Dog();

            DogHuman dongwon = new DogHuman();

            cancho.sleep();
            dongwon.sleep();

            cancho.speak();
            dongwon.speak();

            cancho.walk();
            dongwon.walk();

            dongwon.work();


            Program p = new Program();
            Console.WriteLine(p.sum(1, 2));
            Console.WriteLine(p.sum(1.1f, 2.3f));
            Console.WriteLine(p.sum(1, 2, 3));
        }
    }
}
