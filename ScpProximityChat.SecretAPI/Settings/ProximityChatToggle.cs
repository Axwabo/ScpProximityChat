namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class ProximityChatToggle : CustomKeybindSetting
{

    public ProximityChatToggle() : base(null, Translation.ToggleLabel, KeyCode.LeftAlt, true, false, Translation.ToggleHint)
        => IsShared = SettingsRegistry.Shared;

    protected override CustomSetting CreateDuplicate() => new ProximityChatToggle();

    protected override void HandleSettingUpdate()
    {
        if (IsPressed && KnownOwner!.CanUseProximityChat())
            KnownOwner!.ToggleProximityChat();
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
