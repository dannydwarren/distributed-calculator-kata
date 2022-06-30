using Microsoft.Extensions.Options;
using Worker.Domain;
using Worker.Domain.Configuration;
using ILogger = Worker.Domain.Configuration.ILogger;

Console.WriteLine("test output");

var builder = WebApplication.CreateBuilder(args);

var localConfigOverridesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "src-overrides", "distributed-calculator-kata", "appsettings.json");
builder.Configuration.AddJsonFile(localConfigOverridesPath);

builder.Services.AddControllers();

builder.Services.Configure<Settings>(builder.Configuration.GetSection(nameof(Settings)));
builder.Services.AddSingleton<ISettings>(x => x.GetRequiredService<IOptions<Settings>>().Value);
builder.Services.AddTransient<ILogger, Logger>();
builder.Services.AddTransient<IDistributedCalculatorCoordinator, DistributedCalculatorCoordinator>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

var x = app.Services.GetRequiredService<IRegistrationService>();
await x.RegisterAsync();

app.Run();
