using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class11
{
    public enum State
    {
        Happy, Angry, Sad
    }

    // fsm : 개발자가 정한 상태에 따라 객체의 상태를 가두는 것
    // 상태 패턴: 상태를 객체화 하는 것
    class People
    {
        public string Name;
        private State _state;

        public State State
        {
            get
            {
                switch (_state)
                {
                    case State.Happy:
                        Console.WriteLine($"{Name}은 행복합니다");
                        break;
                    case State.Angry:
                        Console.WriteLine($"{Name}은 화가납니다");
                        break;
                    case State.Sad:
                        Console.WriteLine($"{Name}은 눈물이 납니다");
                        break;
                }
                return _state;
            }
            set
            {
                _state = value; // set에서 들어오는 모든 값은 value로 지정이 됨
            }
        }

        public People(string name)
        {
            Name = name;
        }
    }

    public interface IState
    {
        void Action();
    }

    class Happy : IState
    {
        private string _name;

        public Happy(string name)
        {
            _name = name;
        }
        public void Action()
        {
            Console.WriteLine($"{_name}은 행복합니다 ㅋㅋ");
        }
    }
    class Angry : IState
    {
        private string _name;

        public Angry(string name)
        {
            _name = name;
        }
        public void Action()
        {
            Console.WriteLine($"{_name}은 화가 나서 앵글을 던집니다");
        }
    }
    class Sad : IState
    {
        private string _name;

        public Sad(string name)
        {
            _name = name;
        }
        public void Action()
        {
            Console.WriteLine($"{_name}은 슬퍼서 웁니다");
        }
    }

    class Human
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        // 상태패턴에서 객체를 상태(Key)에 따라 상태객체를(Value) 담아야 합니다.
        public Dictionary<State, IState> stateDictionary;

        public Human(string name)
        {
            _name = name;
            stateDictionary = new Dictionary<State, IState>();
            stateDictionary.Add(State.Happy, new Happy(name));
            stateDictionary.Add(State.Angry, new Angry(name));
            stateDictionary.Add(State.Sad, new Sad(name));
        }
    }
}
