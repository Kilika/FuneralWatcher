using System.Drawing;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic.StaticImageProvider;

public class StaticImageProvider : IImageProvider
{
    public Image GetImage()
    {
        return Image.FromFile("Data/sample_03.png");
    }
}