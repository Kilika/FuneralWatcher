namespace FuneralWatcher.Configuration;

public class ConfigurationRepository : IConfigurationRepository
{
    public IEnumerable<ConfigEntry> Load() => new List<ConfigEntry>
    {
        // eig müsste ich hier alle Elemente aus der json laden und in entsprechende ConfigEntries uebersetzen
        // respektive save eben andersrum
        new ConfigEntry { Category = ConfigurationCategories.Logging, Key = ConfigurationKeys.Level, Value = "Debug" },
        new ConfigEntry { Category = ConfigurationCategories.ImageSettings, Key = ConfigurationKeys.CounterFileName, Value = "MyDeaths.txt" },
        new ConfigEntry { Category = ConfigurationCategories.ImageSettings, Key = ConfigurationKeys.ScreenshotInterval, Value = 2000},
        new ConfigEntry { Category = ConfigurationCategories.ImageSettings, Key = ConfigurationKeys.Pattern, Value = "ihr seid gestorben" },
        new ConfigEntry { Category = ConfigurationCategories.ImageSettings, Key = ConfigurationKeys.ResultPath, Value = "Result\\"}
    };

    public void Save(IEnumerable<ConfigEntry> entriesToStore)
    {
        // erzeuge JSON File und Speicher das als appsettings.
    }

    public void SaveEntry(ConfigEntry entry)
    {
        throw new NotImplementedException();
    }
}