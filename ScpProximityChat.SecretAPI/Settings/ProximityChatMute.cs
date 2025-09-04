namespace ScpProximityChat.SecretAPI.Settings;

public sealed class ProximityChatMute : CustomTwoButtonSetting
{

    public ProximityChatMute() : base(null, "Others' Proximity Chat", "Audible", "Muted", hint: Hints.Mute)
    {
    }

    public bool Muted => IsOptionB;

    protected override CustomSetting CreateDuplicate() => new ProximityChatMute();

    protected override void HandleSettingUpdate()
    {
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
