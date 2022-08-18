using Microsoft.AspNetCore.Mvc;
using Worker.Domain;

namespace Worker.Controllers;

[Route("job")]
public class JobController : ControllerBase
{
    private readonly ICalculateJobWorkflow calculateJobWorkflow;

    public JobController(ICalculateJobWorkflow calculateJobWorkflow)
    {
        this.calculateJobWorkflow = calculateJobWorkflow;
    }
    
    [HttpPost("calculate")]
    public JobResult Calculate([FromBody] Job job)
    {
        var response = calculateJobWorkflow.Calculate(job);

        return response;
    }
}
