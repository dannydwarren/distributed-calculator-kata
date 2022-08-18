namespace Worker.Domain;

public record Job
{
    public Guid JobId { get; init; }
    public string Calculation { get; init; } = "";
}
