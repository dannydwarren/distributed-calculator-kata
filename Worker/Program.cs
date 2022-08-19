using Microsoft.Extensions.Options;
using Worker.Domain;
using Worker.Domain.Configuration;
using ILogger = Worker.Domain.Configuration.ILogger;

var builder = WebApplication.CreateBuilder(args);

Emmersion.Http.DependencyInjectionConfig.ConfigureServices(builder.Services);

var localConfigOverridesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "src-overrides", "distributed-calculator-kata", "appsettings.json");
builder.Configuration.AddJsonFile(localConfigOverridesPath);

builder.Services.AddControllers();

builder.Services.Configure<Settings>(builder.Configuration.GetSection(nameof(Settings)));
builder.Services.AddSingleton<ISettings>(x => x.GetRequiredService<IOptions<Settings>>().Value);
builder.Services.AddTransient<ILogger, Logger>();
builder.Services.AddTransient<ICalculateJobWorkflow, CalculateJobWorkflow>();
builder.Services.AddTransient<IErrorCheckWorkflow, ErrorCheckWorkflow>();
builder.Services.AddTransient<ICalculator, Calculator>();
builder.Services.AddTransient<IDistributedCalculatorCoordinator, DistributedCalculatorCoordinator>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<IJsonSerializer, JsonSerializer>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

var x = app.Services.GetRequiredService<IRegistrationService>();
await x.RegisterAsync();

app.Run();
