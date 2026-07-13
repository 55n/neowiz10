interface Coffee
{
    string Description();
}

class Americano : Coffee
{
    private string Name;

    public Americano()
    {
        Name = "아메리카노";
    }

    public string Description()
    {
        return Name;
    }
}

abstract class CoffeeDecorator : Coffee
{
    protected Coffee coffee;

    protected CoffeeDecorator(Coffee coffee)
    {
        this.coffee = coffee;
    }

    public abstract string Description();
}

class AddSyrup : CoffeeDecorator
{
    public AddSyrup(Coffee coffee) : base(coffee)
    {
        this.coffee = coffee;
    }

    public override string Description()
    {
        return coffee.Description() + " + 시럽"; 

    }
}

class AddSuger : CoffeeDecorator
{
    public AddSuger(Coffee coffee) : base(coffee)
    {
        this.coffee = coffee;
    }

    public override string Description()
    {
       return coffee.Description() + " + 설탕";
    }
}

class Program
{
    static void Main()
    {
        Coffee ame = new Americano();
        ame = new AddSyrup(ame);
        ame = new AddSuger(ame);

        Console.WriteLine(ame.Description());
    }
}
