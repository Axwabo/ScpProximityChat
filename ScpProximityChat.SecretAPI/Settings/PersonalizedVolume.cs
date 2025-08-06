using ScpProximityChat.SecretAPI.Personalization;
using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI.Settings;

public sealed class PersonalizedVolume : CustomSliderSetting
{

    public string Name { get; }
    public string UserId { get; }

    public PersonalizedVolume(Player player) : this(player.Nickname, player.UserId)
    {
    }

    public PersonalizedVolume(string name, string userId) : base(
        PersonalizationManager.IdFor(userId),
        PersonalizationManager.LabelFor(name),
        0,
        100,
        100,
        true,
        "0'%'"
    )
    {
        Name = name;
        UserId = userId;
    }

    public float Volume => SelectedValueInt * 0.01f;

    protected override bool CanView(Player player) => player.UserId != UserId;

    protected override CustomSetting CreateDuplicate() => new PersonalizedVolume(Name, UserId);

    protected override void HandleSettingUpdate() => PersonalizationManager.Configure(KnownOwner!, UserId, Volume);

    public override CustomHeader Header => Headers.Personalization;

    public override int GetHashCode() => Id;

}
