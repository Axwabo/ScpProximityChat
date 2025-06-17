namespace ScpProximityChat.SecretAPI;

public sealed class SecretApiProximityConfig
{

    public bool ShowAvailableHint { get; set; } = true;

    public string AvailableHint { get; set; } = "\n\n\n\nSCP Proximity Chat available.\nPress <mark=#77777755><size=0>.</size><space=0.2em><b>{0}</b><space=0.2em><size=0>.</size></mark> to toggle.";

}
