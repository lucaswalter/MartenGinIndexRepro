namespace SteamDependencyRepro.Models;

/**
 * See 'example_steam_key_value.json` file for a real-world example.
 */
public record SteamKeyValue(
    string? Name,
    string? Value,
    List<SteamKeyValue> Children
);
