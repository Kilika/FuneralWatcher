using System.Drawing;

namespace FuneralWatcher.Logic.Contracts;

public interface IImageRecognizer
{
    Rectangle GetRelevantReadSection(Image img);
}