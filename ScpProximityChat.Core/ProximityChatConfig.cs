using System.ComponentModel;
using PlayerRoles;
using SecretLabNAudio.Core;

namespace ScpProximityChat.Core;

[Serializable]
public sealed class ProximityChatConfig
{

    public bool ShowToggledHint { get; set; } = true;

    public string Enabled { get; set; } = "Proximity Chat enabled.";

    public string Disabled { get; set; } = "Proximity Chat disabled.";

    public SpeakerSettings AudioSettings { get; set; } = SpeakerSettings.Default;

    [Description("Set to null to use the default condition (all SCPs except SCP-079 and SCP-3114)")]
    public List<RoleTypeId>? AllowedRoles { get; set; }

}
