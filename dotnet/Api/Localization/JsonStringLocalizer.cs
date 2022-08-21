using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using System.Text.Json;

public class JsonStringLocalizer : IStringLocalizer
{
    private const string DEFAULT_LANGUAGE_FILE = $"Localization/Languages/en-US.json";
    private readonly IDistributedCache _cache;

    public JsonStringLocalizer(IDistributedCache cache)
    {
        _cache = cache;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetValue(name);

            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var localizedString = this[name];
            if (localizedString.ResourceNotFound) return localizedString;

            return new LocalizedString(name, string.Format(localizedString.Value, arguments), false);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var filePath = $"Localization/Languages/{Thread.CurrentThread.CurrentCulture.Name}.json";
        var jsonStr = File.Exists(filePath) ? File.ReadAllText(filePath) : File.ReadAllText(DEFAULT_LANGUAGE_FILE);
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonStr) ?? new Dictionary<string, string>();

        return dict.Select(x => new LocalizedString(x.Key, x.Value, false));
    }

    private string? GetValue(string key)
    {
        var cacheKey = $"{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        var cacheValue = _cache.GetString(cacheKey);
        if (cacheValue != null) return cacheValue;

        var allStrings = GetAllStrings(true);
        var localizedString = allStrings.FirstOrDefault(x => x.Name == key);
        if (localizedString == null) return null;

        _cache.SetString(cacheKey, localizedString.Value);
        return localizedString.Value;
    }
}