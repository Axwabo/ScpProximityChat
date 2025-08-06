using System.Linq;
using Mirror;
using ScpProximityChat.SecretAPI.Settings;
using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI.Personalization;

public static class PersonalizationManager
{

    private const string Prefix = "Volume for: ";

    private static readonly Dictionary<string, int> IdHashes = [];

    public static int IdFor(string userId)
        => IdHashes.TryGetValue(userId, out var hash)
            ? hash
            : IdHashes[userId] = $"{Prefix}{userId}".GetStableHashCode();

    public static string LabelFor(string name) => $"{Prefix}{name}";

    private static bool IsPersonalized(CustomSetting setting) => setting is PersonalizedVolume;

    public static void Reset()
    {
        IdHashes.Clear();
        var removed = CustomSetting.CustomSettings.RemoveAll(IsPersonalized);
        SettingsRegistry.All.RemoveWhere(IsPersonalized);
        if (removed != 0)
            CustomSetting.ResyncServer();
    }

    public static void Register(Player player)
    {
        if (!player.IsReady)
            return;
        var setting = new PersonalizedVolume(player);
        if (!SettingsRegistry.All.Add(setting))
            return;
        CustomSetting.CustomSettings.Add(setting);
        CustomSetting.ResyncServer();
    }

    public static void Unregister(Player player)
    {
        var setting = CustomSetting.CustomSettings.FirstOrDefault(e => e is PersonalizedVolume volume && volume.UserId == player.UserId);
        if (setting == null)
            return;
        CustomSetting.CustomSettings.Remove(setting);
        SettingsRegistry.All.Remove(setting);
        CustomSetting.ResyncServer();
    }

}
