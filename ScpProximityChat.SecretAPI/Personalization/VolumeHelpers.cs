using System.Linq;
using ScpProximityChat.Core;
using ScpProximityChat.SecretAPI.Settings;
using SecretAPI.Features.UserSettings;
using SecretLabNAudio.Core;

namespace ScpProximityChat.SecretAPI.Personalization;

public static class VolumeHelpers
{

    public static bool CanPersonalize { get; set; }

    public static float Master(Player receiver)
        => CustomSetting.TryGetPlayerSetting(receiver, out MasterVolume? global)
            ? global.Volume
            : ProximityChatPlugin.Cfg.AudioSettings.Volume;

    public static float Personalized(Player receiver, string sourceId)
        => CanPersonalize && CustomSetting.PlayerSettings.TryGetValue(receiver, out var list) && list.FindVolumeSetting(sourceId) is {Volume: var volume}
            ? volume * Master(receiver)
            : Master(receiver);

    public static PersonalizedVolume? FindVolumeSetting(this List<CustomSetting> collection, string userId)
        => collection.OfType<PersonalizedVolume>().FirstOrDefault(e => e.UserId == userId);

    public static void OverrideVolume(this SpeakerPersonalization personalization, Player receiver, float volume)
        => personalization.Override(receiver, SpeakerSettings.From(personalization) with {Volume = volume});

}
