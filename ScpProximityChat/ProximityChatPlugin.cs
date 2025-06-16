using System;
using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;

namespace ScpProximityChat;

public sealed class ProximityChatPlugin : Plugin
{

    public override string Name => "SCP Proximity Chat";
    public override string Description => "Proximity chat for SCPs";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    private readonly EventHandlers _eventHandlers = new();

    public override void Enable() => CustomHandlersManager.RegisterEventsHandler(_eventHandlers);

    public override void Disable() => CustomHandlersManager.UnregisterEventsHandler(_eventHandlers);

}
