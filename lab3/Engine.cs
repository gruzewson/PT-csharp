namespace lab3;

public class Engine
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
}