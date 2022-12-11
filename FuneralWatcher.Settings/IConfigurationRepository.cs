namespace FuneralWatcher.Settings;

public interface IConfigurationRepository
{
    IEnumerable<ConfigEntry> Load();
    void Save(IEnumerable<ConfigEntry> entriesToStore);
    void SaveEntry(ConfigEntry entry);
}