using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace ScpProximityChat;

public sealed class EventHandlers : CustomEventsHandler
{

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        base.OnPlayerLeft(ev);
    }

}
