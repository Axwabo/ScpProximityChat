namespace ScpProximityChat.SecretAPI.Settings;

internal static class SettingsRegistry
{

    public static ProximityChatToggle Toggle { get; } = new();

    public static ProximityChatMute Mute { get; } = new();

    public static MasterVolume Master { get; } = new();

    public static PersonalizationVisibility PersonalizationVisibility { get; } = new();

    public static HashSet<CustomSetting> All { get; } = [Toggle, Mute, Master];

}
