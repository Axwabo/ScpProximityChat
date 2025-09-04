using Mirror;
using SecretLabNAudio.Core;

namespace ScpProximityChat.SecretAPI.Personalization;

public static class PersonalizationManager
{

    private const string Prefix = "Volume for: ";

    private static readonly Dictionary<string, int> IdHashes = [];

    private static readonly Dictionary<string, SpeakerPersonalization> PersonalizationInstances = [];

    public static int IdFor(string userId)
        => IdHashes.TryGetValue(userId, out var hash)
            ? hash
            : IdHashes[userId] = $"{Prefix}{userId}".GetStableHashCode();

    public static string LabelFor(string name) => $"{Prefix}{name}";

    private static bool IsPersonalized(CustomSetting setting) => setting is PersonalizedVolume;

    public static void Reset()
    {
        IdHashes.Clear();
        PersonalizationInstances.Clear();
        var removed = CustomSetting.CustomSettings.RemoveAll(IsPersonalized);
        SettingsRegistry.All.RemoveWhere(IsPersonalized);
        if (removed != 0)
            CustomSetting.ResyncServer();
    }

    public static void Register(Player player)
    {
        if (!player.IsReady || string.IsNullOrEmpty(player.UserId))
            return;
        var setting = new PersonalizedVolume(player);
        if (!SettingsRegistry.All.Add(setting))
            return;
        CustomSetting.CustomSettings.Add(setting);
        CustomSetting.ResyncServer();
    }

    public static void Unregister(Player player)
    {
        if (string.IsNullOrEmpty(player.UserId))
            return;
        PersonalizationInstances.Remove(player.UserId);
        var setting = CustomSetting.CustomSettings.FindVolumeSetting(player.UserId);
        if (setting == null)
            return;
        CustomSetting.CustomSettings.Remove(setting);
        SettingsRegistry.All.Remove(setting);
        CustomSetting.ResyncServer();
    }

    public static void RegisterPersonalization(Player source, SpeakerPersonalization personalization)
    {
        if (!source.IsReady || string.IsNullOrEmpty(source.UserId))
            return;
        PersonalizationInstances[source.UserId] = personalization;
        foreach (var target in Player.ReadyList)
            if (target != source)
                personalization.OverrideVolume(target, VolumeHelpers.Personalized(target, source.UserId));
    }

    public static void Configure(Player receiver, string sourceId, float volume)
    {
        if (PersonalizationInstances.TryGetValue(sourceId, out var personalization))
            personalization.OverrideVolume(receiver, volume * VolumeHelpers.Master(receiver));
    }

    public static void ConfigureAll(Player receiver)
    {
        foreach (var kvp in PersonalizationInstances)
            kvp.Value.OverrideVolume(receiver, VolumeHelpers.Personalized(receiver, kvp.Key));
    }

}
