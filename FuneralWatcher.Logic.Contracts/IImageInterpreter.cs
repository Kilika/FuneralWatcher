using System.Drawing;

namespace FuneralWatcher.Logic.Contracts;

public interface IImageInterpreter
{
    bool ImageContainsPattern(Image img, string pattern);
}