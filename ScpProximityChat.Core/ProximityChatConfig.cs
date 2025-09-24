using System.ComponentModel;
using PlayerRoles;
using SecretLabNAudio.Core;

namespace ScpProximityChat.Core;

/// <summary>Configuration for SCP Proximity Chat.</summary>
[Serializable]
public sealed class ProximityChatConfig
{

    /// <summary>Whether to show a simple hint when a player toggles Proximity Chat.</summary>
    public bool ShowToggledHint { get; set; } = true;

    /// <summary>Message shown when Proximity Chat is enabled.</summary>
    public string Enabled { get; set; } = "Proximity Chat enabled.";

    /// <summary>Message shown when Proximity Chat is disabled.</summary>
    public string Disabled { get; set; } = "Proximity Chat disabled.";

    /// <summary>Audio settings for the speaker toy used for Proximity Chat.</summary>
    public SpeakerSettings AudioSettings { get; set; } = SpeakerSettings.Default;

    /// <summary>List of roles allowed to use Proximity Chat. Set to null to use the default condition (all SCPs except SCP-079 and SCP-3114).</summary>
    [Description("Set to null to use the default condition (all SCPs except SCP-079 and SCP-3114).")]
    public List<RoleTypeId>? AllowedRoles { get; set; }

    /// <summary>Whether the receivers' voice modules should filter whether a player receives the message.</summary>
    [Description("Set to true if the receivers' voice modules should filter whether a player receives the message. Might mess with AudioSettings, but can counter cheaters.")]
    public bool ValidateReceive { get; set; }

    /// <summary>Whether spectators should be able to hear Proximity Chat.</summary>
    [Description("Set to false to disable spectators receiving Proximity Chat audio.")]
    public bool AudibleToSpectators { get; set; } = true;

}
