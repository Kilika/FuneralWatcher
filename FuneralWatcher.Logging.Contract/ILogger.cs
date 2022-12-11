namespace FuneralWatcher.Logging.Contract;

public interface ILogger
{
    void Log(string message);
    void Error(string message);
}