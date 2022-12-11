using System.Drawing;
using FuneralWatcher.Logic.Contract;
using Tesseract;

namespace FuneralWatcher.Logic;

public sealed class ImageInterpreter : IImageInterpreter
{
    private ILogger _logger;

    //private TesseractEngine _tess;
    public ImageInterpreter(ILogger logger)
    {
        _logger = logger;
        //_tess = new TesseractEngine(@"./Data/tessdata", "eng", EngineMode.Default);
    }

    public bool ImageContainsPattern(Image img, string pattern)
    {
        using (var engine = new TesseractEngine("./Data/tessdata", "eng", EngineMode.Default))
        using (var page = engine.Process((Bitmap)img))
        {
            var reading = page.GetText();
            var res = reading.Trim().Equals(pattern, StringComparison.OrdinalIgnoreCase);
            _logger.Log("Read img, " + (res ? "found" : "not found"));
            return res;
        }
        return false;
    }
}