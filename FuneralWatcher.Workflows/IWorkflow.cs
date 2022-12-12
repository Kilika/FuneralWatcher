using FuneralWatcher.CrossCutting;

namespace FuneralWatcher.Workflows
{
    public interface IWorkflow
    {
        event EventHandler<FlankChangeDetected> PatternMatchingFlankDetected;
        Task Run();
    }
}