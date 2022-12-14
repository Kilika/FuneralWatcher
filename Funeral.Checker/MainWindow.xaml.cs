using Emgu.CV;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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
        private Image<Bgr, byte> _input;
        private VideoCapture _vc;
        public MainWindow()
        {
            InitializeComponent();
            
            _emgu = new DeadScreenRecognition(); 
            _input = new Image<Bgr, byte>("Sample/Sample_01.png");   
        }

        private void ShowImage(Image<Bgr, byte> image)
        {
            _input = new Image<Bgr, byte>("Sample/Sample_03.png");
            var res = _emgu.DEBUG(image);
            try
            {
                var show = res.ToBitmapSource();
                imgViewer.Source = show;
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
}