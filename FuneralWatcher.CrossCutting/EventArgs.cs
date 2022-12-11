namespace FuneralWatcher.CrossCutting;

public class FlankChangeDetected : EventArgs
{
    public bool NewDetection { get; }

    public FlankChangeDetected(bool newDetection)
    {
        NewDetection = newDetection;
    }
}