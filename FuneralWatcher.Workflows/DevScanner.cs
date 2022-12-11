using System.ComponentModel;
using System.Drawing;
using FuneralWatcher.CrossCutting;
using FuneralWatcher.Logic.Contract;

namespace FuneralWatcher.Workflows
{
    public sealed class DevScanner : IScanner
    {
        public event EventHandler<FlankChangeDetected>? PatternMatchingFlankDetected;
        
        private IImageProcessor _imageProcessor;
        private IResultProcessor _resultProcessor;
        private IImageInterpreter _imageInterpreter;
        private IImageProvider _imageProvider;
        private bool _found = false;
        
        public DevScanner(IImageProcessor imageProcessor, IResultProcessor resultProcessor, IImageInterpreter imageInterpreter, IImageProvider imageProvider)
        {
            _imageProcessor = imageProcessor;
            _imageInterpreter = imageInterpreter;
            _imageProvider = imageProvider;
            _resultProcessor = resultProcessor;
        }

        public async Task Run()
        {
            CancellationToken cancelToken = new CancellationToken();
            while (!cancelToken.IsCancellationRequested)
            {
                var image = _imageProvider.GetImage();
                var cropped = _imageProcessor.GetCroppedImage(image);
                var found  = _imageInterpreter.ImageContainsPattern(cropped, "You Died");

                if (_found != found)
                {
                    _found = found;
                    PatternMatchingFlankDetected?.Invoke(this, new FlankChangeDetected(found));
                } 
                Thread.Sleep(2000); // TODO: Config
            }
        }

        private async Task ScanScreen(CancellationToken cancelToken)
        {
            await Task.Run(() =>
                {

                }, cancelToken);
        }
    }
}