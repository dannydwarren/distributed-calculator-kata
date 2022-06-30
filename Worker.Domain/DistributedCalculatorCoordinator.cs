using Worker.Domain.Configuration;

namespace Worker.Domain;

public interface IDistributedCalculatorCoordinator
{
    Task<RegistrationResponse> RegisterAsync(Guid workerId, string teamName, string createJobEndPoint , string errorCheckEndpoint);
}

public class DistributedCalculatorCoordinator : IDistributedCalculatorCoordinator
{
    private readonly ILogger logger;

    public DistributedCalculatorCoordinator(ILogger logger)
    {
        this.logger = logger;
    }
    
    public async Task<RegistrationResponse> RegisterAsync(Guid workerId, string teamName, string createJobEndPoint , string errorCheckEndpoint)
    {
        logger.LogInfo($"Danny was here! {nameof(workerId)}:{workerId}; {nameof(teamName)}:{teamName}; {nameof(createJobEndPoint)}:{createJobEndPoint}; {nameof(errorCheckEndpoint)}:{errorCheckEndpoint}");

        throw new Exception(nameof(NotImplementedException));
    }
}

public class RegistrationResponse
{
    public string Result { get; set; }
    public bool IsRegistered { get; set; }
}
