using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Enumeration;
using Microsoft.VisualBasic.CompilerServices;
using FuneralWatcher.Logic.Contract;

namespace FuneralWatcher.Logic;

public class FileResultProcessor : IResultProcessor
{
    // TODO: Config
    private string _fileName = "MyDeaths";
    private string _resultDir = "Result\\"; 

    private ILogger _logger;

    public FileResultProcessor(ILogger logger)
    {
        _logger = logger;
    }

    public void Process()
    {
        if (!Directory.Exists(_resultDir))
            Directory.CreateDirectory(_resultDir);
        
        var fileName = $"{_resultDir}{_fileName}.txt";
        if (!File.Exists(fileName))
        {
            File.Create(fileName);
            File.WriteAllText(fileName, "0");
        }

        var filecontent = File.ReadAllText(fileName);
        if (int.TryParse(filecontent, out int oldValue))
        {
            File.WriteAllText(fileName,(++oldValue).ToString());
            return;
        }
        
        _logger.Error($"cannot read {fileName} with content: {filecontent}");
        File.Copy(fileName, string.Concat(fileName, ".bu"));        
    }
}




