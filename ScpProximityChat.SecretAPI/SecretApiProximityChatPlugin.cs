using Hints;
using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;
using ScpProximityChat.Core;
using ScpProximityChat.SecretAPI.Personalization;
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

    private readonly PersonalizationEventHandlers _handlers = new();

    public override void Enable()
    {
        ProximityChatEvents.Available += SendAvailableHint;
        ProximityChatEvents.Receiving += Receiving;
        CustomSetting.Register(SettingsRegistry.Toggle, SettingsRegistry.Mute);
        if (!Config!.Personalization)
            return;
        PersonalizationManager.DefaultVolume = Config.DefaultVolume;
        SettingsRegistry.All.Add(SettingsRegistry.PersonalizationVisibility);
        CustomSetting.Register(SettingsRegistry.PersonalizationVisibility);
        CustomHandlersManager.RegisterEventsHandler(_handlers);
        ProximityChatEvents.Personalizing += PersonalizationManager.ConfigureAll;
    }

    public override void Disable()
    {
        ProximityChatEvents.Available -= SendAvailableHint;
        ProximityChatEvents.Receiving -= Receiving;
        ProximityChatEvents.Personalizing -= PersonalizationManager.ConfigureAll;
        CustomHandlersManager.UnregisterEventsHandler(_handlers);
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
        if (allow && CustomSetting.TryGetPlayerSetting(target, out ProximityChatMute? mute) && mute.Muted)
            allow = false;
    }

}
