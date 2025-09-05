using System.ComponentModel;

namespace ScpProximityChat.SecretAPI;

/// <summary>Configuration for the SecretAPI-based Proximity Chat extension.</summary>
[Serializable]
public sealed class SecretApiProximityChatConfig
{

    /// <summary>Whether to show a simple hint when Proximity Chat is available.</summary>
    public bool ShowAvailableHint { get; set; } = true;

    /// <summary>The hint text to show when Proximity Chat is available. Use {0} for the toggle keybind.</summary>
    [Description("Use \"{0}\" where the keybind should be displayed.")]
    public string AvailableHint { get; set; } = "\n\n\n\n\n\nSCP Proximity Chat available.\nPress <mark=#77777755><size=0>.</size><space=0.2em><b>{0}</b><space=0.2em><size=0>.</size></mark> to toggle.";

    /// <summary>Whether the same settings should apply on all servers under the same account ID.</summary>
    [Description("Whether the same settings should apply on all servers under the same account ID.")]
    public bool Shared { get; set; } = true;
    
    /// <summary>Whether players should be able to adjust the volume of others' proximity messages.</summary>
    [Description("Whether players should be able to adjust the volume of others' proximity messages.")]
    public bool Personalization { get; set; } = true;

}
