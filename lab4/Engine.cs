namespace lab4;

public interface IComparableEngine : IComparable
{
    double HorsePower { get; }
}

public class Engine : IComparableEngine
{
    
    public Engine(double displacement, double horsePower, string model)
    {
        this.displacement = displacement;
        this.horsePower = horsePower;
        this.model = model;
    }
    
    public Engine()
    {
    }

    private double displacement;
    private double horsePower;
    private string model;

    public double Displacement
    {
        get => displacement;
        set => displacement = value;
    }

    public double HorsePower
    {
        get => horsePower;
        set => horsePower = value;
    }

    public string Model
    {
        get => model;
        set => model = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        Engine otherEngine = obj as Engine;
        if (otherEngine!= null)
        {
            return this.HorsePower.CompareTo(otherEngine.HorsePower);
        }

        return 0;
    }

    public override string ToString()
    {
        return "displacement: " + this.displacement.ToString() + "; HP: " + this.horsePower.ToString() + "; model: " + this.model.ToString();
    }
}