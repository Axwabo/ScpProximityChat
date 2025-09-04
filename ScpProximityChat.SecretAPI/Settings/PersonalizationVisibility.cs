namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class PersonalizationVisibility : CustomTwoButtonSetting
{

    private bool _previouslyVisible;

    public PersonalizationVisibility() : base(null, "Personalization Visibility", "Hidden", "Shown")
    {
    }

    public bool Visible => IsOptionB;

    protected override CustomSetting CreateDuplicate() => new PersonalizationVisibility();

    protected override void HandleSettingUpdate()
    {
        if (_previouslyVisible != Visible)
            SendSettingsToPlayer(KnownOwner!);
        _previouslyVisible = Visible;
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
