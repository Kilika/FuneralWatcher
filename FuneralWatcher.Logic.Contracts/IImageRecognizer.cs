using System.Drawing;

namespace FuneralWatcher.Logic.Contracts;

public interface IImageRecognizer
{
    Image ReduceImageToRelevant(Image img);
}