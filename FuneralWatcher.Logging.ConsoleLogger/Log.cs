using FuneralWatcher.Logging.Contract;

namespace FuneralWatcher.Logging.ConsoleLogger;

public sealed class ConsoleLogger : ILogger
{
    private const string DateTimeFormat = "yyyy.MM.dd hh:mm:ss.fff";

    public void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now.ToString(DateTimeFormat)} : {message}");
    }
    public void Error(string message)
    {
        Console.Error.WriteLine($"{DateTime.Now.ToString(DateTimeFormat)} : {message}");
    }
}