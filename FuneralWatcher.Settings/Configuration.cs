using System.Runtime.CompilerServices;

namespace FuneralWatcher.Settings;

public class Configuration : IConfiguration
{
    private readonly IConfigurationRepository _repository;
    private readonly List<ConfigEntry> _entries;

    public Configuration(IConfigurationRepository repository)
    {
        _repository = repository;
        _entries = repository.Load().ToList();
    }
    
    public T Get<T>(string category, string key)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentNullException(nameof(category));
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var exist = _entries.Any(e => e.Category == category && e.Key == key);
        if (!exist)
            throw new KeyNotFoundException($"{category}/{key}");

        var value = Get<T>(category, key, default(T));
        return value;
    }
    public T Get<T>(string category, string key, T defaultValue)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentNullException(nameof(category));
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var entry = _entries.SingleOrDefault(e => e.Category == category && e.Key == key);
        if (entry is null)
        {
            return defaultValue;
        }

        return (T)entry.Value;
    }

    public void Set<T>(string category, string key, T value, bool persist = false)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentNullException(nameof(category));
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var predicate = new Func<ConfigEntry, bool>(e => e.Category == category && e.Key == key);
        ConfigEntry entry = null;
        if (_entries.Any(predicate))
        {
            entry = _entries.Single(predicate);
            entry.Value = value;
            entry.Persist = persist;
        }
        else
        {
            entry = new ConfigEntry
            {
                Category = category,
                Key = key,
                Persist = persist,
                Value = value,
                Source = null
            };
            _entries.Add(entry);
        }

        if (persist)
        {
            _repository.SaveEntry(entry);
        }
    }
}