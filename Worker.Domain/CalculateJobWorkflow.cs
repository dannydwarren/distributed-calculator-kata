﻿namespace Worker.Domain;

public interface ICalculateJobWorkflow
{
    JobResult Calculate(CalculationJob job);
}

public class CalculateJobWorkflow : ICalculateJobWorkflow
{
    private readonly ICalculator calculator;

    public CalculateJobWorkflow(ICalculator calculator)
    {
        this.calculator = calculator;
    }
    
    public JobResult Calculate(CalculationJob job)
    {
        var calc = calculator.Calculate(job.Calculation);
        var result = new JobResult
        {
            JobId = job.JobId,
            Result = calc
        };

        return result;
    }
}
