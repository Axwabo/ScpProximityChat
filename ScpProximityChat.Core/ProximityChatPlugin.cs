using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;

namespace ScpProximityChat.Core;

public sealed class ProximityChatPlugin : Plugin<ProximityChatConfig>
{

    private static ProximityChatPlugin _instance = null!;

    public static ProximityChatConfig Cfg => _instance.Config!;

    public override string Name => "SCP Proximity Chat";
    public override string Description => "Proximity chat for SCPs";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    private readonly EventHandlers _eventHandlers = new();

    public override void Enable()
    {
        _instance = this;
        ProximityChatEvents.Toggled += ProximityChatEventsOnToggled;
        CustomHandlersManager.RegisterEventsHandler(_eventHandlers);
    }

    public override void Disable()
    {
        ProximityChatEvents.Toggled -= ProximityChatEventsOnToggled;
        CustomHandlersManager.UnregisterEventsHandler(_eventHandlers);
    }

    private void ProximityChatEventsOnToggled(Player player, bool enabled)
    {
        if (Config!.ShowToggledHint)
            player.SendHint(enabled ? Config.Enabled : Config.Disabled);
    }

}
