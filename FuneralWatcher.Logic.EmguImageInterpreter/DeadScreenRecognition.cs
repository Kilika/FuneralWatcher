using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Bioinspired;
using Emgu.CV.CvEnum;
using Emgu.CV.ImgHash;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace FuneralWatcher.Logic.EmguImageInterpreter;

public class DeadScreenRecognition
{
    private Tesseract _tessOcr;

    public DeadScreenRecognition() : this(string.Empty)
    {
    }

    public DeadScreenRecognition(string dataPath)
    {
        if (dataPath == string.Empty)
            dataPath = "./Data/tessdata";

        // _tessOcr = new Tesseract(dataPath, "eng", OcrEngineMode.TesseractOnly);
        // _tessOcr.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ-1234567890");;
    }

    // Image<Bgr, byte>? 
    public IInputArray? DEBUG(Image<Bgr, byte> input)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        // Mat gray = new Mat();
        // Mat sobel = new Mat();

        // var grayImage = gray.ToImage<Gray, byte>();
        //
        // CvInvoke.CvtColor(input, gray, ColorConversion.Bgr2Gray);
        // CvInvoke.Canny(gray, sobel, 100, 50, 3, false);
        //
        // return sobel;
        // Mat se = CvInvoke.GetStructuringElement
        // (Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(10, 2), new Point(-1, -1));

        var res = input.Convert<Gray, byte>();
        // .Sobel(1, 0, 3);
        // .AbsDiff(new Gray(0f));
        // .Convert()
        Mat canny = new Mat(); // = new InputArray();
        CvInvoke.Canny(res, canny, 100, 50);

        var contour = new VectorOfVectorOfPoint();
        Mat m = new Mat();
        CvInvoke.FindContours(canny, contour, m, RetrType.External, ChainApproxMethod.ChainApproxSimple);
        var rectangles = new List<Rectangle>();
        for (int i = 0; i < contour.Size; i++)
        {
            Rectangle rect = CvInvoke.BoundingRectangle(contour[i]);
            double ar = rect.Height / rect.Width;
            if (rect.Height > 110)
            {
                rectangles.Add(rect);
            }
        }

        Image<Bgr, byte> outImage = input.CopyBlank();

        foreach (var r in rectangles)
        {
            CvInvoke.Rectangle(outImage, r, new MCvScalar(0, 255, 255), -5);
        }

        outImage._And(input);

        return outImage;
    }
}