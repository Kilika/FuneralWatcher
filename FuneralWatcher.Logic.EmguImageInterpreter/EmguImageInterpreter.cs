using System.Drawing;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using FuneralWatcher.Configuration;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic.Contract;

namespace FuneralWatcher.Logic.EmguImageInterpreter;

public class EmguImageInterpreter : IImageInterpreter
{
    private ILogger _logger;
    private IConfiguration _config;

    public EmguImageInterpreter(ILogger logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public bool ImageContainsPattern(Image img, string pattern)
    {
        return false;
    }

    public List<Rectangle> DetectLetters(Image<Bgr, Byte> img)
    {
        throw new NotImplementedException();
    }

    private void SaveImg(Image img)
    {
        var dir = _config.Get(ConfigurationCategories.ImageSettings, ConfigurationKeys.ResultPath, "Result\\");
        var filenName = "Foo.png";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        var filePath = string.Concat(dir, filenName);
        img.Save(filePath);
    }
}