using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Enumeration;
using Microsoft.VisualBasic.CompilerServices;
using FuneralWatcher.Logic.Contract;
using FuneralWatcher.Settings;

namespace FuneralWatcher.Logic;

public class FileResultProcessor : IResultProcessor
{
    private string _file;
    private string _resultDir = "Result\\";

    private ILogger _logger;
    private int _deathCounter = 0;

    public FileResultProcessor(ILogger logger, IConfiguration config)
    {
        var fileName = config.Get("ImageSettings", "CounterFileName", "MyDeaths.txt");
        _logger = logger;
        _file = string.Concat(_resultDir, fileName);
        
        InitializeFile();
    }

    private void InitializeFile()
    {
        if (!Directory.Exists(_resultDir))
            Directory.CreateDirectory(_resultDir);

        if (!File.Exists(_file))
        {
            var stream = File.Create(_file);
            stream.Write(new byte[] {0x30},0,1);
            stream.Close();
        }

        if (int.TryParse(File.ReadAllText(_file), out int fileValue))
        {
            _deathCounter = fileValue;
        }
        else
        {
            File.Copy(_file, string.Concat(_file, ".bu"),true);
            _deathCounter = 0;
            File.WriteAllText(_file, 0.ToString());
        }
    }

    public void Process()
    {
        _deathCounter++;
        File.WriteAllText(_file, _deathCounter.ToString());
    }
}