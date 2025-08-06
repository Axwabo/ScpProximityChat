using System.Linq;
using Mirror;
using ScpProximityChat.Core;
using ScpProximityChat.SecretAPI.Settings;
using SecretAPI.Features.UserSettings;
using SecretLabNAudio.Core;

namespace ScpProximityChat.SecretAPI.Personalization;

public static class PersonalizationManager
{

    private const string Prefix = "Volume for: ";

    private static readonly Dictionary<string, int> IdHashes = [];

    private static readonly Dictionary<string, SpeakerPersonalization> PersonalizationInstances = [];

    public static int DefaultVolume { get; set; } = 100;

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
        var setting = CustomSetting.CustomSettings.FirstOrDefault(e => e is PersonalizedVolume volume && volume.UserId == player.UserId);
        if (setting == null)
            return;
        CustomSetting.CustomSettings.Remove(setting);
        SettingsRegistry.All.Remove(setting);
        CustomSetting.ResyncServer();
    }

    public static void ConfigureAll(Player source, SpeakerPersonalization personalization)
    {
        if (!source.IsReady || string.IsNullOrEmpty(source.UserId))
            return;
        PersonalizationInstances[source.UserId] = personalization;
        var defaultSettings = ProximityChatPlugin.Cfg.AudioSettings;
        foreach (var target in Player.ReadyList)
            if (target != source && CustomSetting.TryGetPlayerSetting(target, out PersonalizedVolume? volume))
                personalization.Override(target, defaultSettings with {Volume = volume.Volume});
    }

    public static void Configure(Player receiver, string sourceId, float volume)
    {
        if (PersonalizationInstances.TryGetValue(sourceId, out var personalization))
            personalization.Override(receiver, SpeakerSettings.From(personalization) with {Volume = volume});
    }

}
