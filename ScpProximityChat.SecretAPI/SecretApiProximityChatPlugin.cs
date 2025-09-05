using Hints;
using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;

namespace ScpProximityChat.SecretAPI;

/// <summary>An extension plugin for the Core that adds SSSS-based toggling and personalization.</summary>
public sealed class SecretApiProximityChatPlugin : Plugin<SecretApiProximityChatConfig>
{

    /// <inheritdoc/>
    public override string Name => "SSSS Proximity Chat";

    /// <inheritdoc/>
    public override string Description => "SSSS-based Proximity Chat";

    /// <inheritdoc/>
    public override string Author => "Axwabo";

    /// <inheritdoc/>
    public override Version Version => GetType().Assembly.GetName().Version;

    /// <inheritdoc/>
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    private readonly PersonalizationEventHandlers _handlers = new();

    /// <inheritdoc/>
    public override void Enable()
    {
        ProximityChatEvents.Available += SendAvailableHint;
        ProximityChatEvents.Receiving += Receiving;
        ProximityChatEvents.Personalizing += PersonalizationManager.RegisterPersonalization;
        CustomHandlersManager.RegisterEventsHandler(_handlers);
        CustomSetting.Register(SettingsRegistry.Toggle, SettingsRegistry.Mute, SettingsRegistry.Master);
        SettingsRegistry.Shared = Config!.Shared;
        if (!(VolumeHelpers.CanPersonalize = Config!.Personalization))
            return;
        SettingsRegistry.All.Add(SettingsRegistry.PersonalizationVisibility);
        CustomSetting.Register(SettingsRegistry.PersonalizationVisibility);
    }

    /// <inheritdoc/>
    public override void Disable()
    {
        ProximityChatEvents.Available -= SendAvailableHint;
        ProximityChatEvents.Receiving -= Receiving;
        ProximityChatEvents.Personalizing -= PersonalizationManager.RegisterPersonalization;
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
