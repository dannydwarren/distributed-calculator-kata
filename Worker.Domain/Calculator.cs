namespace Worker.Domain;

public interface ICalculator
{
    int Calculate(string calculation);
}

public class Calculator : ICalculator
{
    public int Calculate(string calculation)
    {
        var parts = calculation.Split('+', StringSplitOptions.TrimEntries);
        return parts.Select(int.Parse).Sum();
    }
}
