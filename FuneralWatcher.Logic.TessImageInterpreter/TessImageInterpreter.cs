using System.Drawing;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic.Contracts;
using Tesseract;

namespace FuneralWatcher.Logic.TessImageInterpreter;

public sealed class TessImageInterpreter : IImageInterpreter
{
    private ILogger _logger;
    private IResultProcessor _resultProcessor;

    //private TesseractEngine _tess;
    public TessImageInterpreter(ILogger logger, IResultProcessor res)
    {
        _logger = logger;
        _resultProcessor = res;
        //_tess = new TesseractEngine(@"./Data/tessdata", "eng", EngineMode.Default);
    }

    public bool ImageContainsPattern(Image img, string pattern)
    {
        _resultProcessor.WriteImageToFilesystem((Bitmap)img, "BeforeReading.png");
        
        using (var engine = new TesseractEngine("./Data/tessdata", "deu", EngineMode.Default))
        {
            engine.SetVariable("tessedit_char_whitelist", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ");
            using (var page = engine.Process(img as Bitmap))
            {
                var found = page.GetText().Trim().Equals(pattern, StringComparison.OrdinalIgnoreCase);
                return found;
            }
        }
    }
}