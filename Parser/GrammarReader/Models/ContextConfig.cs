using Microsoft.VisualBasic;

namespace Parser.GrammarReader.Models;

public class ContextConfig
{
    public bool IncludeSpaces { get; init; }
    public bool UseRegex { get; init; }
    public string EscapeCharacter { get; init; }

    public static ContextConfig FromDict(Dictionary<string, string> dict)
    {
        return new ContextConfig()
        {
            IncludeSpaces = dict.GetBoolOrDefault("spaces"),
            UseRegex = dict.GetBoolOrDefault("regex"),
            EscapeCharacter = dict.GetValueOrDefault("escape", "\\"),
        };
    }

    public Dictionary<string, string> ToDict()
    {
        return new Dictionary<string, string>()
        {
            { "spaces", IncludeSpaces.ToString() },
            { "regex", UseRegex.ToString() }
        };
    }
}

public static class DictExtensions
{
    public static bool GetBoolOrDefault<TKey>(this Dictionary<TKey, string> dict, TKey key) where TKey : notnull
        => bool.TryParse(dict.GetValueOrDefault(key), out var result) && result;
    
    public static int GetIntOrDefault<TKey>(this Dictionary<TKey, string> dict, TKey key) where TKey : notnull
        => int.TryParse(dict.GetValueOrDefault(key), out var result) ? result : default;
}