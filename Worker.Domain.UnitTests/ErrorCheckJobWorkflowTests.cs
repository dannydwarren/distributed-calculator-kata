using Worker.Domain.Configuration;
using Xunit;

namespace Worker.Domain.UnitTests;

public class ErrorCheckJobWorkflowTests : UnitTestBase<ErrorCheckWorkflow>
{
    [Fact]
    public void When_checking()
    {
        var job = new ErrorCheckJob();

        Because(() => ClassUnderTest.Check(job));
        
        It("logs the error", () =>
        {
            GetMock<ILogger>().Verify(x => x.LogInfo($@"Got error check job:
JobId: {job.JobId}
Error Message: {job.ErrorMessage}
---END---"));
        });
    }
}
