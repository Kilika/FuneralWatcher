using System.ComponentModel;
using System.Drawing;
using FuneralWatcher.Configuration;
using FuneralWatcher.CrossCutting;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Workflows
{
    public sealed class DevWorkflow : IWorkflow
    {
        public event EventHandler<FlankChangeDetected>? PatternMatchingFlankChangeDetected;

        private readonly IImageRecognizer _imageRecognizer;
        private readonly IImageInterpreter _imageInterpreter;
        private readonly IImageProvider _imageProvider;
        private readonly IResultProcessor _resultProcessor;
        
        private readonly int _scanInterval;
        private readonly string _searchPattern;
        
        private  bool _found = false;
        
        public DevWorkflow(
            IImageRecognizer imageRecognizer,
            IImageInterpreter imageInterpreter,
            IImageProvider imageProvider,
            IConfiguration configuration, 
            IResultProcessor resultProcessor) 
            
        {
            _imageRecognizer = imageRecognizer;
            _imageInterpreter = imageInterpreter;
            _imageProvider = imageProvider;
            _resultProcessor = resultProcessor;
            _scanInterval = configuration.Get("ImageSettings", "ScreenshotInterval", 1000);
            _searchPattern = configuration.Get("ImageSettings", "Pattern", "YOU DIED");
        }

        public Task Run()
        {
            while (true)
            {
                try
                {
                    var image = _imageProvider.GetImage();
                    _resultProcessor.WriteImageToFilesystem(image, "origin.png");
                    var relevantImage = _imageRecognizer.ReduceImageToRelevant(image);
                    _resultProcessor.WriteImageToFilesystem(relevantImage, "merge.png");
                    var found = _imageInterpreter.ImageContainsPattern(relevantImage, _searchPattern);
                    if (_found != found)
                    {
                        _found = found;
                        PatternMatchingFlankChangeDetected?.Invoke(this, new FlankChangeDetected(found));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                Thread.Sleep(_scanInterval);
            }
            return Task.CompletedTask;
        }
    }
}