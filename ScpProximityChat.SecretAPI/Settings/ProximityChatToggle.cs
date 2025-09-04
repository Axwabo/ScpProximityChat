namespace ScpProximityChat.SecretAPI.Settings;

public sealed class ProximityChatToggle : CustomKeybindSetting
{

    public ProximityChatToggle() : base(null, "Toggle SCP Proximity Chat", KeyCode.LeftAlt, true, false, Hints.Toggle)
    {
    }

    protected override CustomSetting CreateDuplicate() => new ProximityChatToggle();

    protected override void HandleSettingUpdate()
    {
        if (IsPressed && KnownOwner!.CanUseProximityChat())
            KnownOwner!.ToggleProximityChat();
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
