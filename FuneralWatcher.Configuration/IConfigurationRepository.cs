namespace FuneralWatcher.Configuration;

public interface IConfigurationRepository
{
    IEnumerable<ConfigEntry> Load();
    void Save(IEnumerable<ConfigEntry> entriesToStore);
    void SaveEntry(ConfigEntry entry);
}