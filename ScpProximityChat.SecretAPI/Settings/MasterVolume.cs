namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class MasterVolume : VolumeSettingBase
{

    public const string Text = "Master Volume";

    public MasterVolume() : base(Text.GetStableHashCode(), Translation.MasterVolumeLabel, Translation.MasterVolumeHint)
    {
    }

    protected override CustomSetting CreateDuplicate() => new MasterVolume();

    protected override void HandleSettingUpdate() => PersonalizationManager.ConfigureAll(KnownOwner!);

    public override CustomHeader Header => Headers.ProximityChat;

}
