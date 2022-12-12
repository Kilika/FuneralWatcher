using System.Drawing;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic.BasicImageEditor;

public class BasicImageEditor : IImageEditor
{
    public Image CropImage(Image img, Rectangle cropArea)
    {
        var toCrop = new Bitmap(img).Clone(cropArea, img.PixelFormat);
#if DEBUG
        var g = Graphics.FromImage(img);
        g.DrawRectangle(Pens.Aqua, cropArea);
        if (!Directory.Exists("Result"))
            Directory.CreateDirectory("Result");
        img.Save("Result//LastImage.png");
#endif
        return toCrop;
    }
}