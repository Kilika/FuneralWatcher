using System.Drawing;
using FuneralWatcher.Logic.Contract;
using Tesseract;

namespace FuneralWatcher.Logic;

public sealed class ImageInterpreter : IImageInterpreter
{
    private TesseractEngine _tess;
    private ILogger _logger;

    public ImageInterpreter(ILogger logger)
    {
        _logger = logger;
        _tess = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
    }

    public bool ImageContainsPattern(Image img, string pattern)
    {
        var page = _tess.Process((Bitmap)img);
        try
        {
            var reading = page.GetText();
            var res = reading.Trim().Equals(pattern, StringComparison.OrdinalIgnoreCase);
            _logger.Log("Read img, " + (res ? "found" : "not found"));
            return res;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
        finally
        {
            page.Dispose();
        }
        return false;
    }
}