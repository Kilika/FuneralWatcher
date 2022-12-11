﻿using System.ComponentModel;
using System.Drawing;
using FuneralWatcher.CrossCutting;
using FuneralWatcher.Logic.Contract;
using FuneralWatcher.Settings;

namespace FuneralWatcher.Workflows
{
    public sealed class DevScanner : IScanner
    {
        public event EventHandler<FlankChangeDetected>? PatternMatchingFlankDetected;

        private readonly IImageProcessor _imageProcessor;
        private readonly IResultProcessor _resultProcessor;
        private readonly IImageInterpreter _imageInterpreter;
        private readonly IImageProvider _imageProvider;
        private readonly IConfiguration _configuration;

        private int _scanInterval;
        public string _searchPattern;
        private bool _found = false;
        
        public DevScanner(
            IImageProcessor imageProcessor,
            IResultProcessor resultProcessor,
            IImageInterpreter imageInterpreter,
            IImageProvider imageProvider,
            IConfiguration configuration)
        {
            _imageProcessor = imageProcessor;
            _imageInterpreter = imageInterpreter;
            _imageProvider = imageProvider;
            _resultProcessor = resultProcessor;
            _scanInterval = configuration.Get("ImageSettings", "ScreenshotInterval", 1000);
            _searchPattern = configuration.Get("ImageSettings", "Pattern", "YOU DIED");
        }

        public async Task Run()
        {
            CancellationToken cancelToken = new CancellationToken();
            while (!cancelToken.IsCancellationRequested)
            {
                var image = _imageProvider.GetImage();
                var cropped = _imageProcessor.GetCroppedImage(image);
                var found = _imageInterpreter.ImageContainsPattern(cropped, _searchPattern);
                
                if (_found != found)
                {
                    _found = found;
                    PatternMatchingFlankDetected?.Invoke(this, new FlankChangeDetected(found));
                }
                Thread.Sleep(_scanInterval);
            }
        }
    }
}