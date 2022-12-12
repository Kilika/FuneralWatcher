using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using CvEnum = Emgu.CV.CvEnum;
using FuneralWatcher.Configuration;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic.EmguImageInterpreter;

public class EmguImageRecognizer : IImageRecognizer
{
    private ILogger _logger;
    private IImageEditor _imageEditor;

    private string DEBUG_PATH;

    public EmguImageRecognizer(ILogger logger, IConfiguration config, IImageEditor imageEditor)
    {
        _logger = logger;
        _imageEditor = imageEditor;

        var dir = config.Get(ConfigurationCategories.ImageSettings, ConfigurationKeys.ResultPath, "Result\\");
        var filenName = "Foo.png";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        DEBUG_PATH = dir;
    }
    public Rectangle GetRelevantReadSection(Image img)
    {
        var mat = GetMatFromSDImage(img);
        return DetectLetters(mat.ToImage<Bgr, byte>()).First();
    }
    
    private List<Rectangle> DetectLetters(Image<Bgr, Byte> img)
    {
        /*
         * Edege detection via sobel
         * dilation
         * find contours
         * geometrical constaints
         */
        Image<Gray, byte> sobel =
            img.Convert<Gray, byte>()
                .Sobel(1, 0, 3)
                .AbsDiff(new Gray(0.0))
                .Convert<Gray, byte>()
                .ThresholdBinary(new Gray(15), new Gray(255));
        
        sobel.Save($"{DEBUG_PATH}_Step00.png");

        Mat se = CvInvoke.GetStructuringElement(
            shape: CvEnum.ElementShape.Rectangle,
            ksize: new Size(10, 2),
            anchor: new Point(-1, -1));
        
        sobel = sobel.MorphologyEx(
            operation: CvEnum.MorphOp.Dilate, 
            kernel: se, 
            anchor: new Point(-1, -1),
            iterations: 1,
            borderType: CvEnum.BorderType.Reflect, 
            borderValue: new MCvScalar(255));
        sobel.Save($"{DEBUG_PATH}_STEP01.png");
        
        VectorOfVectorOfPoint contour = new VectorOfVectorOfPoint();
        Mat m = new Mat();
        CvInvoke.FindContours(sobel, contour, m, CvEnum.RetrType.External, CvEnum.ChainApproxMethod.ChainApproxSimple);
        List<Rectangle> res = new List<Rectangle>();
        for (int i = 0; i < contour.Size; i++)
        {
            Rectangle rec = CvInvoke.BoundingRectangle(contour[i]);
            double ar = rec.Width / rec.Height;
            if (ar > 2 && rec.Width > 25 && rec.Height > 8 && rec.Height < 100)
            {
                res.Add(rec);
            }
        }

        Image<Bgr, byte> imgOut = img.CopyBlank();
        foreach (var rec in res)
        {
            CvInvoke.Rectangle(img, rec, new MCvScalar(0,0,255), 2);
            CvInvoke.Rectangle(imgOut, rec, new MCvScalar(0,0,255), -1);
        }
        img.Save($"{DEBUG_PATH}_STEP02_Img.png");
        imgOut.Save($"{DEBUG_PATH}_STEP02_ImgOut.png");
        
        return res;
    }
    private Mat GetMatFromSDImage(System.Drawing.Image image)
    {
        int stride = 0;
        Bitmap bmp = new Bitmap(image);

        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
        System.Drawing.Imaging.BitmapData bmpData =
            bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

        System.Drawing.Imaging.PixelFormat pf = bmp.PixelFormat;
        if (pf == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        {
            stride = bmp.Width * 4;
        }
        else
        {
            stride = bmp.Width * 3;
        }

        Image<Bgra, byte> cvImage = new Image<Bgra, byte>(bmp.Width, bmp.Height, stride, (IntPtr)bmpData.Scan0);

        bmp.UnlockBits(bmpData);

        return cvImage.Mat;
    }

}