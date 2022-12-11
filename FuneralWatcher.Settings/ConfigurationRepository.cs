namespace FuneralWatcher.Settings;

public class ConfigurationRepository : IConfigurationRepository
{
    public IEnumerable<ConfigEntry> Load() => new List<ConfigEntry>
    {
        // eig müsste ich hier alle Elemente aus der json laden und in entsprechende ConfigEntries uebersetzen
        // respektive save eben andersrum
        
        
        new ConfigEntry { Category = "Logging", Key = "Level", Value = "Debug" },
        new ConfigEntry { Category = "ImageSettings", Key = "CounterFileName", Value = "MyDeaths.txt" },
        new ConfigEntry { Category = "ImageSettings", Key = "ScreenshotInterval", Value = 2000},
        new ConfigEntry { Category = "ImageSettings", Key = "Pattern", Value = "ihr seid gestorben" }
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