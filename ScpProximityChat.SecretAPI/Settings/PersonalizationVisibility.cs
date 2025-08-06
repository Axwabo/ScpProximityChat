using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI.Settings;

public sealed class PersonalizationVisibility : CustomTwoButtonSetting
{

    private bool _previouslyVisible;

    public PersonalizationVisibility() : base(null, "Personalization Visibility", "Hidden", "Shown")
    {
    }

    protected override CustomSetting CreateDuplicate() => new PersonalizationVisibility();

    protected override void HandleSettingUpdate()
    {
        if (_previouslyVisible != IsDefault)
            SendSettingsToPlayer(KnownOwner!);
        _previouslyVisible = IsDefault;
    }

    public override CustomHeader Header => Headers.ProximityChat;

}
