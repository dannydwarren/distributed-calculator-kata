namespace Worker.Domain;

public record Job
{
    public Guid Id { get; init; }
    public string Calculation { get; init; } = "";
}
