using FuneralWatcher.CrossCutting;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Workflows;

public class ImageEmguWorkflow : IWorkflow
{
    public event EventHandler<FlankChangeDetected>? PatternMatchingFlankChangeDetected;
    private readonly IImageProvider _imageProvider;
    private readonly IImageRecognizer _imageRecognizer;
    
    public ImageEmguWorkflow(IImageProvider imageProvider, IImageRecognizer imageRecognizer)
    {
        _imageProvider = imageProvider;
        _imageRecognizer = imageRecognizer;
    }

    public Task Run()
    {
        var img = _imageProvider.GetImage();
        _imageRecognizer.GetRelevantReadSection(img);
        return Task.CompletedTask;
    }
}