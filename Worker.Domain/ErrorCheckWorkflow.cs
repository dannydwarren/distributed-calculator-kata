using Worker.Domain.Configuration;

namespace Worker.Domain;

public interface IErrorCheckWorkflow
{
    void Check(ErrorCheckJob job);
}

public class ErrorCheckWorkflow : IErrorCheckWorkflow
{
    private readonly ILogger logger;

    public ErrorCheckWorkflow(ILogger logger)
    {
        this.logger = logger;
    }
    
    public void Check(ErrorCheckJob job)
    {
        logger.LogInfo($@"Got error check job:
JobId: {job.JobId}
Error Message: {job.ErrorMessage}
---END---");
    }
}
