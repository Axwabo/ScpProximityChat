using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace ScpProximityChat.SecretAPI.Personalization;

internal sealed class PersonalizationEventHandlers : CustomEventsHandler
{

    public override void OnServerWaitingForPlayers() => PersonalizationManager.Reset();

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        if (VolumeHelpers.CanPersonalize)
            PersonalizationManager.Register(ev.Player);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) => PersonalizationManager.Unregister(ev.Player);

}
