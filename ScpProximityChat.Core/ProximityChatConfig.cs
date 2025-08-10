using System.ComponentModel;
using PlayerRoles;
using SecretLabNAudio.Core;

namespace ScpProximityChat.Core;

[Serializable]
public sealed class ProximityChatConfig
{

    private float _boost;

    public bool ShowToggledHint { get; set; } = true;

    [Description("Global volume multipler (min 0, max 2). Set to 0 to disable boosting.")]
    public float VolumeBoost
    {
        get => _boost;
        set => _boost = Mathf.Clamp(value, 0, 2);
    }

    public string Enabled { get; set; } = "Proximity Chat enabled.";

    public string Disabled { get; set; } = "Proximity Chat disabled.";

    public SpeakerSettings AudioSettings { get; set; } = SpeakerSettings.Default;

    [Description("Set to null to use the default condition (all SCPs except SCP-079 and SCP-3114).")]
    public List<RoleTypeId>? AllowedRoles { get; set; }

    [Description("Set to true if the receivers' voice modules should filter whether a player receives the message. Might mess with AudioSettings, but can counter cheaters.")]
    public bool ValidateReceive { get; set; }

}
