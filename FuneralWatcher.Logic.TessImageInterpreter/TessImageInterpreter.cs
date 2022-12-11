using System.Drawing;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic.Contract;
using Tesseract;

namespace FuneralWatcher.Logic.TessImageInterpreter;

public sealed class TessImageInterpreter : IImageInterpreter
{
    private ILogger _logger;

    //private TesseractEngine _tess;
    public TessImageInterpreter(ILogger logger)
    {
        _logger = logger;
        //_tess = new TesseractEngine(@"./Data/tessdata", "eng", EngineMode.Default);
    }

    public bool ImageContainsPattern(Image img, string pattern)
    {
        using (var engine = new TesseractEngine("./Data/tessdata", "deu", EngineMode.TesseractOnly))
        using (var page = engine.Process((Bitmap)img))
        {
            var reading = page.GetText();
            var res = reading.Trim().Equals(pattern, StringComparison.OrdinalIgnoreCase);
            _logger.Log($"Read img: {reading}, " + (res ? "found pattern" : "dont found pattern"));
            return res;
        }
    }
}