using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp3114;

namespace ScpProximityChat.Core;

/// <summary>Core plugin for speaker-based SCP Proximity chat.</summary>
public sealed class ProximityChatPlugin : Plugin<ProximityChatConfig>
{

    private static ProximityChatPlugin _instance = null!;

    /// <summary>Configuration for the plugin.</summary>
    public static ProximityChatConfig Cfg => _instance.Config!;

    /// <inheritdoc/>
    public override string Name => "SCP Proximity Chat";

    /// <inheritdoc/>
    public override string Description => "Proximity chat for SCPs";

    /// <inheritdoc/>
    public override string Author => "Axwabo";

    /// <inheritdoc/>
    public override Version Version => GetType().Assembly.GetName().Version;

    /// <inheritdoc/>
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    /// <inheritdoc/>
    public override LoadPriority Priority => LoadPriority.High;

    private readonly EventHandlers _eventHandlers = new();

    /// <inheritdoc/>
    public override void Enable()
    {
        _instance = this;
        ProximityChatEvents.Toggled += OnToggled;
        CustomHandlersManager.RegisterEventsHandler(_eventHandlers);
        VolumeBoost.Amount = Config!.VolumeBoost;
        var allowedRoles = Config!.AllowedRoles;
        if (allowedRoles == null)
            ProximityChatState.Conditions.Add(player => player.RoleBase is FpcStandardScp and not Scp3114Role);
        else
            ProximityChatState.Conditions.Add(player => allowedRoles.Contains(player.Role));
    }

    /// <inheritdoc/>
    public override void Disable()
    {
        ProximityChatEvents.Toggled -= OnToggled;
        CustomHandlersManager.UnregisterEventsHandler(_eventHandlers);
    }

    private void OnToggled(Player player, bool enabled)
    {
        if (Config!.ShowToggledHint)
            player.SendHint(enabled ? Config.Enabled : Config.Disabled);
    }

}
