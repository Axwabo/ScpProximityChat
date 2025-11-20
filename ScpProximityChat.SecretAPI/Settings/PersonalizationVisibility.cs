namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class PersonalizationVisibility : CustomTwoButtonSetting
{

    private bool _previouslyVisible;

    public PersonalizationVisibility() : base(null, Translation.PersonalizationVisibility, Translation.Shown, Translation.Hidden)
        => IsShared = SettingsRegistry.Shared;

    public bool Visible => IsOptionB;

    protected override CustomSetting CreateDuplicate() => new PersonalizationVisibility();

    protected override void HandleSettingUpdate()
    {
        if (_previouslyVisible != Visible)
            ResyncToOwner();
        _previouslyVisible = Visible;
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
