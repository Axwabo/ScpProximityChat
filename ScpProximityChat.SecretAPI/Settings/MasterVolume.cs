namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class MasterVolume : VolumeSettingBase
{

    public MasterVolume() : base(null, "Master Volume", ProximityChatPlugin.Cfg.AudioSettings.Volume, "Overall volume of SCP Proximity Chat.")
    {
    }

    protected override CustomSetting CreateDuplicate() => new MasterVolume();

    protected override void HandleSettingUpdate() => PersonalizationManager.ConfigureAll(KnownOwner!);

    public override CustomHeader Header => Headers.ProximityChat;

}
