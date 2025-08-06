using Hints;
using LabApi.Loader.Features.Plugins;
using ScpProximityChat.Core;
using ScpProximityChat.SecretAPI.Settings;
using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI;

public sealed class SecretApiProximityChatPlugin : Plugin<SecretApiProximityChatConfig>
{

    public override string Name => "SSSS Proximity Chat";
    public override string Description => "SSSS-based proximity chat";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    public override void Enable()
    {
        ProximityChatEvents.Available += SendAvailableHint;
        ProximityChatEvents.Receiving += Receiving;
        CustomSetting.Register(SettingsRegistry.Toggle, SettingsRegistry.Mute);
        if (!Config!.Personalization)
            return;
        SettingsRegistry.All.Add(SettingsRegistry.PersonalizationVisibility);
        CustomSetting.Register(SettingsRegistry.PersonalizationVisibility);
    }

    public override void Disable()
    {
        ProximityChatEvents.Available -= SendAvailableHint;
        ProximityChatEvents.Receiving -= Receiving;
        CustomSetting.UnRegister(SettingsRegistry.All);
    }

    private void SendAvailableHint(Player player)
    {
        if (Config!.ShowAvailableHint)
            player.SendHint(
                Config.AvailableHint,
                [new SSKeybindHintParameter(SettingsRegistry.Toggle.Id)],
                HintEffectPresets.FadeInAndOut(0.95f),
                10
            );
    }

    private static void Receiving(Player sender, Player target, ref bool allow)
    {
        if (allow && CustomSetting.TryGetPlayerSetting(target, out ProximityChatMute? mute) && !mute.IsDefault)
            allow = false;
    }

}
