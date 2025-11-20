namespace ScpProximityChat.SecretAPI;

[Serializable]
public sealed class SecretApiProximityChatTranslations
{

    public string ChatHeader { get; set; } = "Proximity Chat";

    public string PersonalizationHeader { get; set; } = "Proximity Volume Personalization";

    public string ToggleLabel { get; set; } = "Toggle SCP Proximity Chat";

    public string ToggleHint { get; set; } = "Enable/disable using Proximity Chat while you're an SCP.";

    public string Mute { get; set; } = "Enable/disable hearing others' SCP Proximity Chat.";

}
