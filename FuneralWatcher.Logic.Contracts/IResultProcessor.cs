using System.Drawing;

namespace FuneralWatcher.Logic.Contracts;

public interface IResultProcessor
{
    void Process();

    void WriteImageToFilesystem(Image Img, string Name);
}