using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using VoiceChat;

namespace ScpProximityChat.Core;

internal sealed class EventHandlers : CustomEventsHandler
{

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        ev.Player.DisableProximityChat();
        VolumeBoost.Remove(ev.Player);
    }

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (!ev.Player.CanUseProximityChat())
        {
            ev.Player.DisableProximityChat();
            return;
        }

        if (!ev.Player.IsProximityChatEnabled())
            ProximityChatEvents.OnAvailable(ev.Player);
    }

    public override void OnPlayerSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
    {
        if (ev.Message.Channel != VoiceChatChannel.ScpChat || !ProximityChatState.ActiveSpeakers.TryGetValue(ev.Player, out var speaker))
            return;
        ev.IsAllowed = false;
        var send = true;
        ProximityChatEvents.OnSending(ev.Player, ref send);
        var message = VolumeBoost.Convert(ev.Player, ev.Message);
        message.ControllerId = speaker.ControllerId;
        var validate = ProximityChatPlugin.Cfg.ValidateReceive;
        foreach (var player in Player.ReadyList)
        {
            if (player == ev.Player)
                continue;
            var allow = !validate || player.VoiceModule?.ValidateReceive(ev.Player.ReferenceHub, VoiceChatChannel.Proximity) != VoiceChatChannel.None;
            ProximityChatEvents.OnReceiving(ev.Player, player, ref allow);
            if (allow)
                player.Connection.Send(message);
        }
    }

    public override void OnServerWaitingForPlayers() => ProximityChatState.ActiveSpeakers.Clear();

}
