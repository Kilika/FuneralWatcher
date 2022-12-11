using System.Drawing;

namespace FuneralWatcher.Logic.Contract;

public interface IImageInterpreter
{
    bool ImageContainsPattern(Image img, string pattern);
}