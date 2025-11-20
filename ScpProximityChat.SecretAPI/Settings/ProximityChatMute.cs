namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class ProximityChatMute : CustomTwoButtonSetting
{

    public ProximityChatMute() : base(null, Translation.MuteLabel, Translation.Audible, Translation.Muted, hint: Translation.MuteHint)
        => IsShared = SettingsRegistry.Shared;

    public bool Muted => IsOptionB;

    protected override CustomSetting CreateDuplicate() => new ProximityChatMute();

    protected override void HandleSettingUpdate()
    {
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
