using FuneralWatcher.CrossCutting;

namespace FuneralWatcher.Workflows
{
    public interface IScanner
    {
        event EventHandler<FlankChangeDetected> PatternMatchingFlankDetected;
        Task Run();
    }
}