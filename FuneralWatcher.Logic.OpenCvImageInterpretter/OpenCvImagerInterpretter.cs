using System.Drawing;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic.Contract;

namespace FuneralWatcher.Logic.OpenCvImageInterpretter;

public class OpenCVImagerInterpretter : IImageInterpreter
{
    private ILogger _logger;


    public OpenCVImagerInterpretter(ILogger logger)
    {
        _logger = logger;
    }

    public bool ImageContainsPattern(Image img, string pattern)
    {
        return false;
    }

    // public List<Rectangle> DetectLetters(Image<Bgr, Byte> img)
    // {
    //     
    // }
}