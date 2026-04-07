namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class PersonalizationVisibility : CustomTwoButtonSetting
{

    public const string Text = "Personalization Visibility";

    public PersonalizationVisibility() : base(Text.GetStableHashCode(), Translation.VisibilityLabel, Translation.Shown, Translation.Hidden)
        => IsShared = SettingsRegistry.Shared;

    public bool Visible => IsOptionA;

    protected override CustomSetting CreateDuplicate() => new PersonalizationVisibility();

    protected override void HandleSettingUpdate()
    {
        if (HasValueChanged)
            ResyncToOwner();
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
