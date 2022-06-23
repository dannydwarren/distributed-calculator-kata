namespace Worker.Domain.Configuration;

public interface ISettings
{
    Guid WorkerId { get; }
    string TeamName { get; }
    string CreateJobEndpoint { get; }
    string ErrorCheckEndpoint { get; }
}

public class Settings : ISettings
{
    public Guid WorkerId { get; set; }
    public string TeamName { get; set;  }
    public string CreateJobEndpoint { get; set; }
    public string ErrorCheckEndpoint { get; set; }
}
