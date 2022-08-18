namespace Worker.Domain;

public record JobResult
{
    public Guid JobId { get; init; }
    public int Result { get; init; }
}
