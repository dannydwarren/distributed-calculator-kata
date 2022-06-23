namespace Worker.Domain.Configuration;

public interface ILogger
{
    void LogInfo(string message);
}

public class Logger : ILogger
{
    public void LogInfo(string message)
    {
        Console.WriteLine(message);
    }
}
