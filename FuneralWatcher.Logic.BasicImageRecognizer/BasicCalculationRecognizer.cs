using System.Drawing;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic.ImageRecognizer;

#pragma warning disable CA1416
public sealed class BasicCalculationRecognizer : IImageRecognizer
{
    private Rectangle _calculatedRectangle;
    private IImageEditor _imageEditor;

    public BasicCalculationRecognizer(IImageEditor imageEditor)
    {
        _imageEditor = imageEditor;
    }

    public Rectangle GetRelevantReadSection(Image img)
    {
        var originSize = img.Size;
        // var cornerX = originSize.Width / 100 * 30;
        var cornerX = 5;
        var cornerY = originSize.Height / 100 * 45;
        // var sizeX = originSize.Width / 100 * 42;
        var sizeX = 1910;
        var sizeY = originSize.Height / 100 * 25;

        Point corner = new Point(cornerX, cornerY);         // left upper corner: (x:26%, y:40%)
        Size size = new Size(sizeX, sizeY);                 // width: 48% , height: 10 %
        
        return new Rectangle(corner, size);
    }

    public Image GetCroppedImage(Image img, Rectangle rect)
    {
        if (_calculatedRectangle == new Rectangle())
            _calculatedRectangle = GetRelevantReadSection(img);
        
        return _imageEditor.CropImage(img, _calculatedRectangle);
    }
}
#pragma warning restore CA1416
