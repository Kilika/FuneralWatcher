using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Ccm;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic.EmguImageInterpreter;

public class DeadScreenRecognition : IImageRecognizer
{
    private Image<Bgra, byte> _inputArray;
    private readonly IResultProcessor _resultProcessor;
    private string _emguDir = "emgu\\";
    public DeadScreenRecognition(IResultProcessor resultProcessor)
    {
        _resultProcessor = resultProcessor;
    }

    public Image ReduceImageToRelevant(Image img)
    {
        _inputArray = ConvertImageToInputArray((Bitmap)img);
        var recs = GetLetterRectangles(_inputArray);
        var resultImage = MutliplyImageWithSections(_inputArray, recs);
        var asBitmap = Convert(resultImage).Convert<Gray, byte>().ToBitmap();
        asBitmap.Save(_resultProcessor.GetResultDir() + _emguDir + "asBitmap.png");
        return asBitmap;
    }

    private void ShowImageInWindows(Image<Bgra, byte> input)
    {
        var name = "yourThing";
        CvInvoke.NamedWindow(name);
        var matrix = input.Mat;
        CvInvoke.Resize(matrix, matrix, new Size(640, 360));
        CvInvoke.Imshow(name, matrix);
        CvInvoke.WaitKey(0);
    }

    private Image<Bgra, byte> ConvertImageToInputArray(Bitmap inputImg)
    {
        int stride = 0;
        Rectangle rect = new Rectangle(0, 0, inputImg.Width, inputImg.Height);
        BitmapData bmpData = inputImg.LockBits(rect, ImageLockMode.ReadWrite, inputImg.PixelFormat);
        Image<Bgra, byte> cvImage = new Image<Bgra, byte>(inputImg.Width, inputImg.Height, bmpData.Stride, (IntPtr)bmpData.Scan0);
        inputImg.UnlockBits(bmpData);
        return cvImage;
    }

    private Image<Bgra, byte> MutliplyImageWithSections(Image<Bgra, byte> inputImage, IEnumerable<Rectangle> sections)
    {
        var outImage = inputImage.CopyBlank();
        foreach (var r in sections)
        {
            CvInvoke.Rectangle(outImage, r, new MCvScalar(0, 255, 255), -2);
        }
        outImage._And(inputImage);
        //ShowImageInWindows(outImage);
        return outImage;
    }
    
    private IList<Rectangle> GetLetterRectangles(Image<Bgra, byte> input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        var res = input.Convert<Gray, byte>();
        var canny = new Mat();
        CvInvoke.Canny(res, canny, 100, 50);

        var contour = new VectorOfVectorOfPoint();
        var m = new Mat();
        CvInvoke.FindContours(canny, contour, m, RetrType.External, ChainApproxMethod.ChainApproxSimple);
        var rectangles = new List<Rectangle>();
        for (var i = 0; i < contour.Size; i++)
        {
            var rect = CvInvoke.BoundingRectangle(contour[i]);
            if (rect.Height > 120) // letter high
            {
                rectangles.Add(rect);
            }
        }

        return rectangles;
    }

    private Image<Bgr, byte> Convert(Image<Bgra, byte> input)
    {
        Image<Bgr, byte> converted = new Image<Bgr, byte>(input.Size);
        CvInvoke.CvtColor(input, converted, ColorConversion.Bgra2Bgr);
        return converted;
    }
}