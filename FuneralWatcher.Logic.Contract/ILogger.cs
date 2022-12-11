namespace FuneralWatcher.Logic.Contract;

public interface ILogger
{
    void Log(string message);
    void Error(string message);
}