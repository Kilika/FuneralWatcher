using FuneralWatcher.CrossCutting;

namespace FuneralWatcher.Workflows
{
    public interface IWorkflow
    {
        event EventHandler<FlankChangeDetected> PatternMatchingFlankChangeDetected;
        Task Run();
    }
}