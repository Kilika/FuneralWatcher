using System.Drawing;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic.WindowsScreenCastImageProvider;

public sealed class WindowsScreenCastImageProvider : IImageProvider
{
    public Image GetImage()
    {
        // Mache Screenshot
        var inMemoryGraphics = new Bitmap(1920, 1080);
        var graphics = Graphics.FromImage(inMemoryGraphics);
        graphics.CopyFromScreen(0,0,0,0,new Size(1920, 1080));
        return inMemoryGraphics;
    }
}