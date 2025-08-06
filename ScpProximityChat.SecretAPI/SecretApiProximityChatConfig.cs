using System.ComponentModel;

namespace ScpProximityChat.SecretAPI;

[Serializable]
public sealed class SecretApiProximityChatConfig
{

    public bool ShowAvailableHint { get; set; } = true;

    public string AvailableHint { get; set; } = "\n\n\n\n\n\nSCP Proximity Chat available.\nPress <mark=#77777755><size=0>.</size><space=0.2em><b>{0}</b><space=0.2em><size=0>.</size></mark> to toggle.";

    [Description("Whether players can adjust the volume of others' proximity messages.")]
    public bool Personalization { get; set; } = true;

    [Description("Default volume percentage if personalization is enabled.")]
    public int DefaultVolume { get; set; } = 100;

}
