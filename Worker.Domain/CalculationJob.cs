namespace Worker.Domain;

public record CalculationJob
{
    public Guid JobId { get; init; }
    public string Calculation { get; init; } = "";
}

public record ErrorCheckJob
{
    public Guid JobId { get; init; }
    public string ErrorMessage { get; init; } = "";
}
