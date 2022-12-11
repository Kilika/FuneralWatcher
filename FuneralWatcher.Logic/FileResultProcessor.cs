using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Enumeration;
using Microsoft.VisualBasic.CompilerServices;
using FuneralWatcher.Logic.Contract;
using FuneralWatcher.Settings;

namespace FuneralWatcher.Logic;

public class FileResultProcessor : IResultProcessor
{
    private string _fileName;
    private string _resultDir = "Result\\"; 

    private ILogger _logger;

    public FileResultProcessor(ILogger logger, IConfiguration config)
    {
        _logger = logger;
        _fileName = config.Get("ImageSettings", "CounterFileName", "MyDeaths.txt");
    }

    public void Process()
    {
        if (!Directory.Exists(_resultDir))
            Directory.CreateDirectory(_resultDir);
        
        var fileName = $"{_resultDir}{_fileName}";
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




