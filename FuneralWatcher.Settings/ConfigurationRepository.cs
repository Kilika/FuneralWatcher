namespace FuneralWatcher.Settings;

public class ConfigurationRepository : IConfigurationRepository
{
    public IEnumerable<ConfigEntry> Load() => new List<ConfigEntry>
    {
        new ConfigEntry { Category = "Logging", Key = "Level", Value = "Debug" },
        new ConfigEntry { Category = "ImageSettings", Key = "CounterFileName", Value = "MyDeaths.txt" },
        new ConfigEntry { Category = "ImageSettings", Key = "ScreenshotInterval", Value = 2000}
    };

    public void Save(IEnumerable<ConfigEntry> entriesToStore)
    {
        throw new NotImplementedException();
    }

    public void SaveEntry(ConfigEntry entry)
    {
        throw new NotImplementedException();
    }
}