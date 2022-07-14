using Emmersion.Http;
using Worker.Domain.Configuration;
using HttpMethod = Emmersion.Http.HttpMethod;

namespace Worker.Domain;

public interface IDistributedCalculatorCoordinator
{
    Task<RegistrationResponse> RegisterAsync(Guid workerId, string teamName, string createJobEndPoint,
        string errorCheckEndpoint);
}

public class DistributedCalculatorCoordinator : IDistributedCalculatorCoordinator
{
    public const string SuccessMessageMagicalIdentifier = "ready to accept"; 
    
    private readonly IHttpClient httpClient;
    private readonly ISettings settings;
    private readonly IJsonSerializer jsonSerializer;

    public DistributedCalculatorCoordinator(IHttpClient httpClient, ISettings settings, IJsonSerializer jsonSerializer)
    {
        this.httpClient = httpClient;
        this.settings = settings;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task<Domain.RegistrationResponse> RegisterAsync(Guid workerId, string teamName, string createJobEndPoint,
        string errorCheckEndpoint)
    {
        var requestJson = new RegistrationRequest
        {
            WorkerId = workerId,
            TeamName = teamName,
            CreateJobEndpoint = createJobEndPoint,
            ErrorCheckEndpoint = errorCheckEndpoint
        };

        var headers = new HttpHeaders();
        headers.Add("Content-Type", "application/json");
        
        var request = new HttpRequest
        {
            Method = HttpMethod.POST,
            Url = $"{settings.CoordinatorBaseUrl}/register",
            Headers = headers,
            Body =  jsonSerializer.Serialize(requestJson)
        };

        var httpResponse = await httpClient.ExecuteAsync(request);

        var response = jsonSerializer.Deserialize<RegistrationResponse>(httpResponse.Body);

        if (!response.Result.Contains(SuccessMessageMagicalIdentifier))
        {
            throw new Exception(response.Result);
        }

        return new Domain.RegistrationResponse
        {
            Result = response.Result,
            IsRegistered = true
        };
    }

    public class RegistrationRequest
    {
        public Guid WorkerId { get; set; }
        public string TeamName { get; set; }
        public string CreateJobEndpoint { get; set; }
        public string ErrorCheckEndpoint { get; set; }
    }
    
    public class RegistrationResponse
    {
        public string Result { get; set; }
    }
}

public class RegistrationResponse
{
    public string Result { get; set; }
    public bool IsRegistered { get; set; }
}
