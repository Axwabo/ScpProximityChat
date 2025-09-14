using System.Linq;
using SecretLabNAudio.Core;

namespace ScpProximityChat.SecretAPI.Personalization;

/// <summary>Helper methods for accessing volume settings of players.</summary>
public static class VolumeHelpers
{

    /// <summary>Whether players can personalize the volume of other players.</summary>
    public static bool CanPersonalize { get; set; }

    /// <summary>Gets the master volume of SCP Proximity Chat for the given player.</summary>
    /// <param name="receiver">The player whose master volume to get.</param>
    /// <returns>The master volume of SCP Proximity Chat for the given player in range 0-1.</returns>
    public static float Master(Player receiver)
        => CustomSetting.TryGetPlayerSetting(receiver, out MasterVolume? global)
            ? global.Volume
            : ProximityChatPlugin.Cfg.AudioSettings.Volume;

    /// <summary>Gets the personalized volume of SCP Proximity Chat for the given receiver and source User ID, accounting for master volume.</summary>
    /// <param name="receiver">The player receiving the audio.</param>
    /// <param name="sourceId">The User ID of the player sending the audio.</param>
    /// <returns>The personalized volume of SCP Proximity Chat for the given receiver and source User ID in range 0-1.</returns>
    public static float Personalized(Player receiver, string sourceId)
        => CanPersonalize && CustomSetting.PlayerSettings.TryGetValue(receiver, out var list) && list.FindVolumeSetting(sourceId) is {Volume: var volume}
            ? volume * Master(receiver)
            : Master(receiver);

    internal static PersonalizedVolume? FindVolumeSetting(this List<CustomSetting> collection, string userId)
        => collection.OfType<PersonalizedVolume>().FirstOrDefault(e => e.UserId == userId);

    /// <summary>Overrides only the volume setting for the given receiver.</summary>
    /// <param name="personalization">The personalization instance to modify.</param>
    /// <param name="receiver">The player whose volume to override.</param>
    /// <param name="volume">The new volume in range 0-1.</param>
    public static void OverrideVolume(this SpeakerPersonalization personalization, Player receiver, float volume)
        => personalization.Override(receiver, SpeakerSettings.From(personalization) with {Volume = volume});

}
