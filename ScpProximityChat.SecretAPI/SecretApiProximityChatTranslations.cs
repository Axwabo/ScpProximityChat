namespace ScpProximityChat.SecretAPI;

[Serializable]
public sealed class SecretApiProximityChatTranslations
{

    public string ChatHeader { get; set; } = "Proximity Chat";

    public string PersonalizationHeader { get; set; } = "Proximity Volume Personalization";

    public string ToggleLabel { get; set; } = "Toggle SCP Proximity Chat";

    public string ToggleHint { get; set; } = "Enable/disable using Proximity Chat while you're an SCP.";

    public string MuteLabel { get; set; } = "Others' Proximity Chat";

    public string MuteHint { get; set; } = "Enable/disable hearing others' SCP Proximity Chat.";

    public string Audible { get; set; } = "Audible";

    public string Muted { get; set; } = "Muted";

    public string PersonalizationVisibility { get; set; } = "Personalization Visibility";

    public string Shown { get; set; } = "Shown";

    public string Hidden { get; set; } = "Hidden";

    public string MasterVolumeLabel { get; set; } = "Master Volume";

    public string MasterVolumeHint { get; set; } = "Overall volume of SCP Proximity Chat.";

    public string VolumeFor { get; set; } = "Volume for: ";

}
