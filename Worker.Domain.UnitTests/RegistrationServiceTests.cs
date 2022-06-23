using System;
using System.Threading.Tasks;
using Moq;
using Shouldly;
using Worker.Domain.Configuration;
using Xunit;

namespace Worker.Domain.UnitTests;

public class RegistrationServiceTests : UnitTestBase<RegistrationService>
{
    [Fact]
    public async Task When_registration_succeeds()
    {
        var workerId = NewGuid();
        var teamName = RandomString();
        var createJobEndpoint = RandomString();
        var errorCheckEndpoint = RandomString();
        GetMock<ISettings>().SetupGet(x => x.WorkerId).Returns(workerId);
        GetMock<ISettings>().SetupGet(x => x.TeamName).Returns(teamName);
        GetMock<ISettings>().SetupGet(x => x.CreateJobEndpoint).Returns(createJobEndpoint);
        GetMock<ISettings>().SetupGet(x => x.ErrorCheckEndpoint).Returns(errorCheckEndpoint);
        
        var registrationResponse = new RegistrationResponse
        {
            IsRegistered = true,
            Result = RandomString()
        };
        GetMock<IDistributedCalculatorCoordinator>()
            .Setup(x => x.RegisterAsync(workerId, teamName, createJobEndpoint, errorCheckEndpoint))
            .ReturnsAsync(registrationResponse);

        await BecauseAsync(() => ClassUnderTest.RegisterAsync());

        It("logs registration result", () =>
        {
            GetMock<ILogger>().Verify(x => x.LogInfo(registrationResponse.Result));
        });
    }

    [Fact]
    public async Task When_registration_fails()
    {
        var registrationResponse = new RegistrationResponse
        {
            IsRegistered = false,
            Result = RandomString()
        };

        GetMock<IDistributedCalculatorCoordinator>().Setup(x => x.RegisterAsync(IsAny<Guid>(), IsAny<string>(), IsAny<string>(), IsAny<string>()))
            .ReturnsAsync(registrationResponse);
        
        var exception = await BecauseThrowsAsync<Exception>(() => ClassUnderTest.RegisterAsync());
                
        It("logs registration result", () =>
        {
            GetMock<ILogger>().Verify(x => x.LogInfo(registrationResponse.Result));
        });
        
        It("includes the failure reason in the exception", () =>
        {
            exception.ShouldNotBeNull().Message.ShouldBe(registrationResponse.Result);
        });
    }
}
