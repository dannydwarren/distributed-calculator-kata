namespace Worker.Domain;

public record JobResult
{
    public int Result { get; init; }
    public Guid JobId { get; init; }
}
