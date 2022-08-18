using System;
using Shouldly;
using Xunit;

namespace Worker.Domain.UnitTests;

public class CalculateJobWorkflowTests : UnitTestBase<CalculateJobWorkflow>
{
    private readonly Random rand = new();
    
    [Fact]
    public void When_calculating_a_job()
    {
        var job = new Job
        {
            JobId = NewGuid(),
            Calculation = RandomString()
        };
        var expectedResult = rand.Next();
        
        GetMock<ICalculator>().Setup(x => x.Calculate(job.Calculation)).Returns(expectedResult);
        
        var response = Because(() => ClassUnderTest.Calculate(job));
        
        It("returns the result for the correct job", () =>
        {
            response.JobId.ShouldBe(job.JobId);
        });
        
        It("returns the result of the calculation", () =>
        {
            response.Result.ShouldBe(expectedResult);
        });
    }
}
