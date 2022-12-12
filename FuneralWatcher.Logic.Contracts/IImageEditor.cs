using System.Drawing;

namespace FuneralWatcher.Logic.Contracts;

public interface IImageEditor
{
    Image CropImage(Image img, Rectangle cropArea);
}