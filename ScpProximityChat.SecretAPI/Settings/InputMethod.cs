namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class InputMethod : CustomTwoButtonSetting
{

    public const string Text = "Input Method";

    public InputMethod() : base(Text.GetStableHashCode(),
        Translation.InputMethodLabel,
        Translation.Toggle,
        Translation.Hold,
        hint: Translation.InputMethodHint
    ) => IsShared = SettingsRegistry.Shared;

    public bool Hold => IsOptionB;

    protected override CustomSetting CreateDuplicate() => new InputMethod();

    protected override void HandleSettingUpdate()
    {
        if (!Hold)
            ProximityChatToggle.InvalidateCancellation(KnownOwner!);
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
