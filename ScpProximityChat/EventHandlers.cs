using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp3114;
using Utils.Networking;
using VoiceChat.Networking;

namespace ScpProximityChat;

public sealed class EventHandlers : CustomEventsHandler
{

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) => ev.Player.DisableProximityChat();

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (ev.NewRole is not (FpcStandardScp and not Scp3114Role))
            ev.Player.DisableProximityChat();
    }

    public override void OnPlayerSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
    {
        if (!ProximityChatState.ActiveSpeakers.TryGetValue(ev.Player, out var speaker))
            return;
        ev.IsAllowed = false;
        new AudioMessage(speaker.ControllerId, ev.Message.Data, ev.Message.DataLength).SendToAuthenticated();
    }

}
