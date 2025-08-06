using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI.Settings;

public sealed class PersonalizationVisibility : CustomTwoButtonSetting
{

    public PersonalizationVisibility() : base(null, "Personalization Visibility", "Hidden", "Shown")
    {
    }

    protected override CustomSetting CreateDuplicate() => new PersonalizationVisibility();

    protected override void HandleSettingUpdate() => SendSettingsToPlayer(KnownOwner!);

    public override CustomHeader Header => Headers.ProximityChat;

}
