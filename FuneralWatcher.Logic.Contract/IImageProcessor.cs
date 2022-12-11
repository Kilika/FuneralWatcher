using System.Drawing;

namespace FuneralWatcher.Logic.Contract;

public interface IImageProcessor
{
    Image GetCroppedImage(Image img);
}