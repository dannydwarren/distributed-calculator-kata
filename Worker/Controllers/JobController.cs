using Microsoft.AspNetCore.Mvc;
using Worker.Domain;

namespace Worker.Controllers;

[Route("job")]
public class JobController : ControllerBase
{
    private readonly ICalculateJobWorkflow calculateJobWorkflow;
    private readonly IErrorCheckWorkflow errorCheckWorkflow;

    public JobController(ICalculateJobWorkflow calculateJobWorkflow,
        IErrorCheckWorkflow errorCheckWorkflow)
    {
        this.calculateJobWorkflow = calculateJobWorkflow;
        this.errorCheckWorkflow = errorCheckWorkflow;
    }
    
    [HttpPost("calculate")]
    public JobResult Calculate([FromBody] CalculationJob job)
    {
        var response = calculateJobWorkflow.Calculate(job);

        return response;
    }
    
    [HttpPost("error-check")]
    public IActionResult ErrorCheck([FromBody] ErrorCheckJob job)
    {
        errorCheckWorkflow.Check(job);

        return Ok();
    }
}
