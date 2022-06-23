using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Worker.Controllers;

[Route("diagnostic")]
public class DiagnosticController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck()
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        var machineName = Environment.MachineName;
        return Ok(@$"Assembly Version: {assemblyVersion}
Machine Name: {machineName}
X-Original-Host: {Request.Headers["x-original-host"].FirstOrDefault()}
");
    }
}
