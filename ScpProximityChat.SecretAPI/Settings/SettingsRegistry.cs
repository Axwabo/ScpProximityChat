using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI.Settings;

internal static class SettingsRegistry
{

    private static readonly HashSet<CustomSetting> All = [];

    public static ProximityChatToggle Toggle { get; } = new();

    public static IReadOnlyCollection<CustomSetting> AllSettings => All;

}
