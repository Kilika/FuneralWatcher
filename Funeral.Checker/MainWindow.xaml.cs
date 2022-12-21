using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FuneralWatcher.Logic.EmguImageInterpreter;
using Microsoft.Win32;

namespace Funeral.Checker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeadScreenRecognition _emgu;
        // private Image<Bgr, byte> _input;
        private Image _input;
        public MainWindow()
        {
            InitializeComponent();
            
            _emgu = new DeadScreenRecognition(null); 
            _input = Image.FromFile("Sample/Sample_01.png");   
        }

        private void ShowImage(Image image)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Image Files (*.bmp;*.png;*.jpg)|*.bmp;*.png;*.jpg";
            
            string filename = "Sample/Sample_01.png";
            if (ofd.ShowDialog() == true)
            {
                filename = ofd.FileName;
            }
            _input = Image.FromFile(filename);
            var res = _emgu.ReduceImageToRelevant(_input);
            
            try
            {
                var show = res;
                imgViewer.Source = show.ToImageSource();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                imgViewer.Source = null;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ShowImage(_input);
        }
    }

    public static class ImageExtension
    {
        public static ImageSource ToImageSource(this Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}