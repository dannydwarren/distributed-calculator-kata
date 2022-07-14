using System;
using System.Threading.Tasks;
using Emmersion.Http;
using Moq;
using Shouldly;
using Worker.Domain.Configuration;
using Xunit;

namespace Worker.Domain.UnitTests;

public class DistributedCalculatorCoordinatorTests : UnitTestBase<DistributedCalculatorCoordinator>
{
    //error check: {"result":"You must provide valid URIs for the CreateJob and ErrorCheck endpoints"}
    
    [Fact]
    public async Task When_posting_registration()
    {
        Guid workerId = NewGuid();
        string teamName = RandomString();
        string createJobEndpoint = RandomString();
        string errorCheckEndpoint = RandomString();
        string coordinatorBaseUrl = RandomString();
        string registrationRequestJson = RandomString();
        string registrationResponseJson = RandomString();
        var httpResponse = new HttpResponse(200, new HttpHeaders(), registrationResponseJson);
        var registrationResponse = new DistributedCalculatorCoordinator.RegistrationResponse
        {
            Result = $"{RandomString()}{DistributedCalculatorCoordinator.SuccessMessageMagicalIdentifier}"
        };

        GetMock<ISettings>().SetupGet(x => x.CoordinatorBaseUrl).Returns(coordinatorBaseUrl);
        
        DistributedCalculatorCoordinator.RegistrationRequest capturedRegistrationRequest = null;
        GetMock<IJsonSerializer>().Setup(x => x.Serialize(IsAny<DistributedCalculatorCoordinator.RegistrationRequest>()))
            .Callback<DistributedCalculatorCoordinator.RegistrationRequest>(registrationRequest => capturedRegistrationRequest = registrationRequest)
            .Returns(registrationRequestJson);

        HttpRequest capturedRequest = null;
        GetMock<IHttpClient>().Setup(x => x.ExecuteAsync(IsAny<IHttpRequest>()))
            .Callback<IHttpRequest>(request => capturedRequest = (HttpRequest)request)
            .ReturnsAsync(httpResponse);

        GetMock<IJsonSerializer>().Setup(x =>
            x.Deserialize<DistributedCalculatorCoordinator.RegistrationResponse>(registrationResponseJson))
            .Returns(registrationResponse);

        var result = await BecauseAsync(
            () => ClassUnderTest.RegisterAsync(workerId, teamName, createJobEndpoint, errorCheckEndpoint));

        It("prepares the data", () =>
        {
            capturedRegistrationRequest.ShouldNotBeNull().ShouldSatisfyAllConditions(x =>
            {
                x.WorkerId.ShouldBe(workerId);
                x.TeamName.ShouldBe(teamName);
                x.CreateJobEndpoint.ShouldBe(createJobEndpoint);
                x.ErrorCheckEndpoint.ShouldBe(errorCheckEndpoint);
            });
        });
        
        It("makes the API call", () =>
        {
            capturedRequest.ShouldNotBeNull().ShouldSatisfyAllConditions(x =>
            {
                x.Method.ShouldBe(HttpMethod.POST);
                x.Url.ShouldBe($"{coordinatorBaseUrl}/register");
                x.Body.ShouldBe(registrationRequestJson);
                x.Headers.GetValue("Content-Type").ShouldBe("application/json");
            });
        });
        
        It("returns the result", () =>
        {
            result.ShouldNotBeNull().ShouldSatisfyAllConditions(x =>
            {
               x.Result.ShouldBe(registrationResponse.Result);
               x.IsRegistered.ShouldBeTrue();
            });
        });
    }

    [Fact]
    public async Task When_registration_throws()
    {
        string expectedErrorReason = RandomString();
        
        string coordinatorBaseUrl = RandomString();
        string registrationRequestJson = RandomString();
        string registrationResponseJson = RandomString();
        var httpResponse = new HttpResponse(200, new HttpHeaders(), registrationResponseJson);
        var registrationResponse = new DistributedCalculatorCoordinator.RegistrationResponse
        {
            Result = expectedErrorReason
        };

        GetMock<ISettings>().SetupGet(x => x.CoordinatorBaseUrl).Returns(coordinatorBaseUrl);
        
        GetMock<IJsonSerializer>().Setup(x => x.Serialize(IsAny<DistributedCalculatorCoordinator.RegistrationRequest>()))
            .Returns(registrationRequestJson);

        GetMock<IHttpClient>().Setup(x => x.ExecuteAsync(IsAny<IHttpRequest>()))
            .ReturnsAsync(httpResponse);

        GetMock<IJsonSerializer>().Setup(x =>
                x.Deserialize<DistributedCalculatorCoordinator.RegistrationResponse>(registrationResponseJson))
            .Returns(registrationResponse);
        
        var exception = await BecauseThrowsAsync<Exception>(() => ClassUnderTest.RegisterAsync(NewGuid(), RandomString(), RandomString(), RandomString()));
        
        It("gives the reason", () =>
        {
            exception.ShouldNotBeNull().ShouldSatisfyAllConditions(x =>
            {
                x.Message.ShouldBe(expectedErrorReason);
            });
        });
    }
}
