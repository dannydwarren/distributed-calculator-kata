using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Worker.Domain.UnitTests;

public class CalculateJobWorkflowTests : UnitTestBase<CalculateJobWorkflow>
{
    private readonly Random rand = new();
    
    [Fact]
    public async Task When_calculating_a_job()
    {
        var job = new Job
        {
            Id = NewGuid()
        };
        var expectedResult = rand.Next();
        
        GetMock<ICalculator>().Setup(x => x.Calculate(IsAny<string>())).Returns(expectedResult);
        
        var response = await BecauseAsync(() => ClassUnderTest.CalculateAsync(job));
        
        It("returns the result for the correct job", () =>
        {
            response.JobId.ShouldBe(job.Id);
        });
        
        It("returns the result of the calculation", () =>
        {
            response.Result.ShouldBe(expectedResult);
        });
    }
}
