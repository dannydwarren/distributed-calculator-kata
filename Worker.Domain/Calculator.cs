namespace Worker.Domain;

public interface ICalculator
{
    int Calculate(string calculation);
}

public class Calculator : ICalculator
{
    public int Calculate(string calculation)
    {
        var noSpace = calculation.Replace(" ", "").Replace("CALCULATE:", "");
        var convertedSubtraction = noSpace.Replace("-", "+-1*").TrimStart('+').Replace("*+", "*").Replace("/+", "/");
        var additionParts = convertedSubtraction.Split('+', StringSplitOptions.TrimEntries);
        var multiplicationParts = additionParts.Select(x => x.Split('*', StringSplitOptions.TrimEntries));
        var divisionParts = multiplicationParts.Select(x => x.Select(y => y.Split('/', StringSplitOptions.TrimEntries)));
        var result = divisionParts.Select(x => x.Select( y=>
        {
            return y.Select(int.Parse).Aggregate((a, b) => a / b);
        }).Aggregate(1, (a, b) => a * b)).Sum();
        return result;
    }
}
