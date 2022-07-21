namespace Worker.Domain;

public class CalculateJobWorkflow
{
    private readonly ICalculator calculator;

    public CalculateJobWorkflow(ICalculator calculator)
    {
        this.calculator = calculator;
    }
    
    public async Task<JobResult> CalculateAsync(Job job)
    {
        return new JobResult
        {
            JobId = job.Id,
            Result = calculator.Calculate("")
        };
    }
}
