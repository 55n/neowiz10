using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 객체지향의 요소(Object Oriented Programming)
// 1. 캡슐화, 2. 상속, 3. 다형성, 4. 추상화
// 지킬 것(Class를 객체로 보자는 것)
// 1. Single Responsibility Principle - 클래스는 하나의 책임만
// 2. Open-Closed Principle - 기존 코드를 수정하지 않고 새 기능을 추가할 수 있게 설계
// 3. Liskov Substitution Principle - 자식 클래스는 부모 클래스를 대체할 수 있어야 함
// 4. Interface Segregation Principle - 클라이언트는 사용하지 않는 인터페이스(진짜 interface)에 의존하면 안 됨
// 5. Dependency Inversion Principle - 모듈은 추상화에 의존해야 함

// 접근제한자 - public, private, protected(부모의 protected는 자식만 사용 가능)

// ▶클래스의 private 변수에 외부 클래스가 접근하는 법◀
// 1. 생성자 패턴
// 2. setter/getter 패턴
// 3. 프로퍼티 패턴

// 분류       클래스     구조체
// 상속       O           X
// 메서드     O           O
// new        O           O|X
// call       by Ref      by Val



namespace Class6
{
    struct Student_S
    {
        public int age;
        public string name;

        public void veiwStudent()
        {
            Console.WriteLine($"이름 : {name} | 나이 : {age}");
        }
    }

    class Student_C
    {
        public int age;
        public string name;

        public void veiwStudent()
        {
            Console.WriteLine($"이름 : {name} | 나이 : {age}");
        }
    }

    class A
    {
        public Student_S student_s;
        public Student_C student_c;

        public void main()
        {
            student_c = new Student_C();
            student_c.name = "김종찬";
            student_c.age = 31;

            student_s = new Student_S();
            student_s.name = "김현호";
            student_s.age = 33;
        }

        public void viewStudent()
        {
            Console.WriteLine("[Class]");
            student_c.veiwStudent();

            Console.WriteLine("[Struct]");
            student_s.veiwStudent();
        }
    }

    class PlayerInfo
    {
        public string name;
        public int level;
        private int hp;
        private int mp;

        // 프로퍼티 패턴 ============================================================
        public int HP { get; private set; }


        // 생성자 패턴 ==============================================================
        public PlayerInfo(string name, int level, int hp, int mp)
        {
            this.name = name;
            this.level = level;
            this.hp = hp;
            this.mp = mp;
        }

        // getter/setter 패턴 =======================================================
        public void setHp(int hp)
        {
            this.hp = hp;
        }

        public int getHp(int hp)
        {
            return this.hp;
        }

        public void setMp(int mp)
        {
            this.mp = mp;
        }

        public int getMp(int mp)
        {
            return this.mp;
        }


        public void printPlayerInfo()
        {
            Console.WriteLine($"{name} 의 레벨은 {level} 이고 HP: {hp} | MP: {mp} 입니다");
        }

        public void demageHp(int atk)
        {
            Console.WriteLine("플레이어가 공격받았다!!");
            HP -= atk;
            Console.WriteLine($"현재 체력: {HP}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PlayerInfo playerInfo = new PlayerInfo("한", 200, 100, 50);

            playerInfo.name = "han";
            playerInfo.level = 200;
            playerInfo.setHp(10000);
            playerInfo.setMp(5000);

            playerInfo = new PlayerInfo("한", 200, 100, 50);

            playerInfo.printPlayerInfo();

            Console.WriteLine(playerInfo.HP); // public get, private set 즉 Program 클래스에서 HP에 대한 setter 접근 불가

            Console.WriteLine("플레이어 공격");
            playerInfo.demageHp(10);

            A a = new A();
            a.main();
            a.viewStudent();

            Console.WriteLine("[Class]");
            a.student_c.veiwStudent();

            Console.WriteLine("[Struct]");
            a.student_s.veiwStudent();

            Student_C student_c = a.student_c;
            Student_S student_s = a.student_s;

            student_c.veiwStudent();
            student_s.veiwStudent();

            student_c.name = "양성은";
            student_c.age = 26;

            student_s.name = "한덕현";
            student_s.age = 34;

            student_c.veiwStudent();
            student_s.veiwStudent();

            Console.WriteLine("______________________A 클래스 상황_______________________");
            a.student_c.veiwStudent();
            a.student_s.veiwStudent();
        }
    }
}
