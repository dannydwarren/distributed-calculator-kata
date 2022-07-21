namespace Worker.Domain;

public interface ICalculator
{
    int Calculate(string calculation);
}

public class Calculator : ICalculator
{
    public int Calculate(string calculation)
    {
        throw new NotImplementedException();
    }
}
