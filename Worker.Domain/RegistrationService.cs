using Worker.Domain.Configuration;

namespace Worker.Domain;

public interface IRegistrationService
{
    Task RegisterAsync();
}

public class RegistrationService : IRegistrationService
{
    private readonly ISettings settings;
    private readonly IDistributedCalculatorCoordinator distributedCalculatorCoordinator;
    private readonly ILogger logger;

    public RegistrationService(ISettings settings,
        IDistributedCalculatorCoordinator distributedCalculatorCoordinator,
        ILogger logger)
    {
        this.settings = settings;
        this.distributedCalculatorCoordinator = distributedCalculatorCoordinator;
        this.logger = logger;
    }

    public async Task RegisterAsync()
    {
        var response = await distributedCalculatorCoordinator.RegisterAsync(settings.WorkerId, settings.TeamName,
            settings.CreateJobEndpoint, settings.ErrorCheckEndpoint);

        logger.LogInfo(response.Result);

        if (!response.IsRegistered)
        {
            throw new Exception(response.Result);
        }
    }
}
