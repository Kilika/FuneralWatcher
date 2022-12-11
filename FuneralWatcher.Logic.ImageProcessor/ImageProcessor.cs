﻿using System.Drawing;
using System.Net.Mime;
using FuneralWatcher.Logic.Contract;

namespace FuneralWatcher.Logic.ImageProcessor;

#pragma warning disable CA1416
public sealed class ImageProcessor : IImageProcessor
{
    private Rectangle _calculatedRectangle;
    
    public Image GetCroppedImage(Image img)
    {
        if (_calculatedRectangle == new Rectangle())
            _calculatedRectangle = CalculateCenterRectangle(img);
        
        return Crop(img, _calculatedRectangle);
    }

    private Image Crop(Image img, Rectangle croppingArea)
    {
        var toCrop = new Bitmap(img).Clone(croppingArea, img.PixelFormat);
        var g = Graphics.FromImage(img);
        g.DrawRectangle(Pens.Aqua, croppingArea);
        if (!Directory.Exists("Result"))
            Directory.CreateDirectory("Result");
        img.Save("Result//LastImage.png");

        return toCrop;
    }

    private Rectangle CalculateCenterRectangle(Image img)
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
}
#pragma warning restore CA1416
