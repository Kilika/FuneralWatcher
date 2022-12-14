using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace Funeral.Checker;

public static class Extensions
{
    /// <summary>
    /// Delete a GDI object
    /// </summary>
    /// <param name="o">The pointer to the GDI object to be deleted</param>
    /// <returns></returns>
    [DllImport("gdi32")]
    private static extern int DeleteObject(IntPtr o);
    
    public static void ToMat(this BitmapSource source, Mat image)
    {
        if (source.Format == PixelFormats.Bgra32)
        {
            image.Create(source.PixelHeight, source.PixelWidth, DepthType.Cv8U, 4);
            source.CopyPixels(Int32Rect.Empty, image.DataPointer, image.Step * image.Rows, image.Step);
        }
        else if (source.Format == PixelFormats.Bgr24)
        {
            image.Create(source.PixelHeight, source.PixelWidth, DepthType.Cv8U, 3);
            source.CopyPixels(Int32Rect.Empty, image.DataPointer, image.Step * image.Rows, image.Step);
        }
        else
        {
            throw new Exception(String.Format("Conversion from BitmapSource of format {0} is not supported.",
                source.Format));
        }
    }

    public static BitmapSource ToBitmapSource(this IInputArray? image)
    {
        using (InputArray ia = image.GetInputArray())
        using (Mat m = ia.GetMat())
        using (System.Drawing.Bitmap source = m.ToBitmap())
        {
            IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr); //release the HBitmap
            return bs;
        }
    }
}