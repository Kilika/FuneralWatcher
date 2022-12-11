namespace FuneralWatcher.Configuration;

public sealed class ConfigurationCategories
{
    public const string ImageSettings = nameof(ImageSettings);
    public const string Logging = nameof(Logging);
}

public sealed class ConfigurationKeys
{
    public const string Level = nameof(Level);
    
    
    public const string Pattern = nameof(Pattern);
    public const string CounterFileName = nameof(CounterFileName);
    public const string ScreenshotInterval = nameof(ScreenshotInterval);
    public const string ResultPath = nameof(ResultPath);
}