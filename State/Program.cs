
interface IZombieState
{
    void execute(Zombie zombie);
}

class Chase : IZombieState
{
    public void execute(Zombie zombie)
    {
        // 조건에 따라 대상의 상태 변경
        if(zombie.playerDistance < 5)
        {
            Console.WriteLine("추적 끝 공격 시작");
            zombie.SetState(new Attack());
        }
        else if(zombie.playerDistance >= 20)
        {
            Console.WriteLine("추적 끝 로밍 시작");
            zombie.SetState(new Roam());
        }
    }
}

class Roam : IZombieState
{
    public void execute(Zombie zombie)
    {
        if(zombie.playerDistance < 20)
        {
            Console.WriteLine("로밍 끝 추적 시작");
            zombie.SetState(new Chase());
        }
    }
}

class Attack : IZombieState
{
    public void execute(Zombie zombie)
    {
        if(zombie.playerDistance >= 5)
        {
            Console.WriteLine("공격 끝 추적 시작");
            zombie.SetState(new Chase());
        }
    }
}


class Zombie // 상태 변경 트리거
{
    public int playerDistance;
    public IZombieState state;

    public Zombie()
    {
        playerDistance = 50;
        state = new Roam();
    }

    public void SetState(IZombieState state)
    {
        this.state = state;
    }

    public void stateUpdate()
    {
        state.execute(this);
    }
}

class Program
{
    static void Main()
    {
        Zombie zombie = new Zombie();

        while (true)
        {
            Console.WriteLine("플레이어와의 거리: ");
            int distance = int.Parse(Console.ReadLine());

            zombie.playerDistance = distance;

            zombie.stateUpdate();
        }
    }
}
