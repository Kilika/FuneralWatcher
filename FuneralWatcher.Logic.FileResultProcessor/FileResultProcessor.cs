using System.Drawing;
using FuneralWatcher.Configuration;
using FuneralWatcher.Logging.Contract;
using FuneralWatcher.Logic.Contracts;

namespace FuneralWatcher.Logic;

public class FileResultProcessor : IResultProcessor
{
    private string _deathCounterFile;
    private string _resultDir;

    private ILogger _logger;
    private int _deathCounter = 0;

    public FileResultProcessor(ILogger logger, IConfiguration config)
    {
        var fileName = config.Get(ConfigurationCategories.ImageSettings, ConfigurationKeys.CounterFileName, "MyDeaths.txt");
        _resultDir = config.Get(ConfigurationCategories.ImageSettings, ConfigurationKeys.ResultPath, "Result\\");
        _logger = logger;
        _deathCounterFile = string.Concat(_resultDir, fileName);
        
        InitializeFile();
    }

    private void InitializeFile()
    {
        if (!Directory.Exists(_resultDir))
            Directory.CreateDirectory(_resultDir);

        if (!File.Exists(_deathCounterFile))
        {
            var stream = File.Create(_deathCounterFile);
            stream.Write(new byte[] {0x30},0,1);
            stream.Close();
        }

        if (int.TryParse(File.ReadAllText(_deathCounterFile), out int fileValue))
        {
            _deathCounter = fileValue;
        }
        else
        {
            File.Copy(_deathCounterFile, string.Concat(_deathCounterFile, ".bu"),true);
            _deathCounter = 0;
            File.WriteAllText(_deathCounterFile, "0");
        }
    }

    public void Process()
    {
        _deathCounter++;
        File.WriteAllText(_deathCounterFile, _deathCounter.ToString());
    }

    public void WriteImageToFilesystem(Image Img, string Name)
    {
        Img.Save(_resultDir+Name);
    }

    public string GetResultDir()
    {
        return _resultDir;
    }
}