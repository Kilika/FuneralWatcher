using System.ComponentModel;
using System.Drawing;
using FuneralWatcher.Configuration;
using FuneralWatcher.CrossCutting;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Workflows
{
    public sealed class DevWorkflow : IWorkflow
    {
        public event EventHandler<FlankChangeDetected>? PatternMatchingFlankDetected;

        private readonly IImageRecognizer _imageRecognizer;
        private readonly IImageInterpreter _imageInterpreter;
        private readonly IImageProvider _imageProvider;
        private readonly IImageEditor _editor;

        private readonly int _scanInterval;
        private readonly string _searchPattern;
        
        private  bool _found = false;
        
        public DevWorkflow(
            IImageRecognizer imageRecognizer,
            IImageInterpreter imageInterpreter,
            IImageProvider imageProvider,
            IConfiguration configuration, 
            IImageEditor editor)
        {
            _imageRecognizer = imageRecognizer;
            _imageInterpreter = imageInterpreter;
            _imageProvider = imageProvider;
            _editor = editor;
            _scanInterval = configuration.Get("ImageSettings", "ScreenshotInterval", 1000);
            _searchPattern = configuration.Get("ImageSettings", "Pattern", "YOU DIED");
        }

        public async Task Run()
        {
            //CancellationToken cancelToken = new CancellationToken();
            while (true)
            {
                try
                {
                    var image = _imageProvider.GetImage();
                    var cropped = _editor.CropImage(image, _imageRecognizer.GetRelevantReadSection(image));
                    var found = _imageInterpreter.ImageContainsPattern(cropped, _searchPattern);
                    if (_found != found)
                    {
                        _found = found;
                        PatternMatchingFlankDetected?.Invoke(this, new FlankChangeDetected(found));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                Thread.Sleep(_scanInterval);
            }
        }
    }
}