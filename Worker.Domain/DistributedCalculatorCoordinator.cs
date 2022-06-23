namespace Worker.Domain;

public interface IDistributedCalculatorCoordinator
{
    Task<RegistrationResponse> RegisterAsync(Guid workerId, string teamName, string createJobEndPoint , string errorCheckEndpoint);
}

public class DistributedCalculatorCoordinator : IDistributedCalculatorCoordinator
{
    public Task<RegistrationResponse> RegisterAsync(Guid workerId, string teamName, string createJobEndPoint , string errorCheckEndpoint)
    {
        throw new NotImplementedException();
    }
}

public class RegistrationResponse
{
    public string Result { get; set; }
    public bool IsRegistered { get; set; }
}
