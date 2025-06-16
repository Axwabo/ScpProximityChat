using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using VoiceChat;
using VoiceChat.Networking;

namespace ScpProximityChat;

public sealed class EventHandlers : CustomEventsHandler
{

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) => ev.Player.DisableProximityChat();

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (!ev.Player.CanUseProximityChat())
            ev.Player.DisableProximityChat();
    }

    public override void OnPlayerSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
    {
        if (ev.Message.Channel != VoiceChatChannel.ScpChat || !ProximityChatState.ActiveSpeakers.TryGetValue(ev.Player, out var speaker))
            return;
        ev.IsAllowed = false;
        var message = new AudioMessage(speaker.ControllerId, ev.Message.Data, ev.Message.DataLength);
        foreach (var player in Player.ReadyList)
            if (player != ev.Player)
                player.Connection.Send(message);
    }

}
