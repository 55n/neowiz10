using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *      분류              virtual             abstract            interface
 *      재정의             강제X               무조건           상속 받으면 무조건
 *     미리구현가능           O                   x                   X
 *     다중상속               X                   X                   O
 *     추상화대상         메서드               클래스               몰?루
 */

namespace Class7
{
    abstract class Animal
    {
        public abstract void sleep();

        public virtual void speak() // 결국 override 되냐 안 되냐로 일반 메서드와 갈림
        {
            Console.WriteLine("speak~~");
        }
    }

    public interface IWalk
    {
        void walk();
    }

    public interface IWork
    {
        void work();
    }

    class Dog : Animal, IWalk
    {
        public override void sleep()
        {
            Console.WriteLine("쿨쿨");
        }

        public override void speak()
        {
            base.speak();

            Console.WriteLine("낑낑");
        }

        public void walk()
        {
            Console.WriteLine("사뿐사뿐");
        }
    }

    class DogHuman : Animal, IWork, IWalk
    {
        public override void sleep()
        {
            Console.WriteLine("드르렁~");
        }

        public override void speak()
        {
            // base.speak();
            Console.WriteLine("사람이 말을 하면");
        }

        public void walk()
        {
            Console.WriteLine("터덜...터덜....");
        }

        public void work()
        {
            Console.WriteLine("집에 가고 싶다");
        }
    }

    class Override
    {
    }
}
