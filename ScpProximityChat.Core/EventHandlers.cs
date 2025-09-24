using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using PlayerRoles;
using VoiceChat;
using VoiceChat.Networking;

namespace ScpProximityChat.Core;

internal sealed class EventHandlers : CustomEventsHandler
{

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) => ev.Player.DisableProximityChat();

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
        var message = new AudioMessage(speaker.ControllerId, ev.Message.Data, ev.Message.DataLength);
        var validate = ProximityChatPlugin.Cfg.ValidateReceive;
        var spectators = ProximityChatPlugin.Cfg.AudibleToSpectators;
        foreach (var player in Player.ReadyList)
            if (player != ev.Player && AllowReceive(ev, player, validate, spectators))
                player.Connection.Send(message);
    }

    public override void OnServerWaitingForPlayers() => ProximityChatState.ActiveSpeakers.Clear();

    private static bool AllowReceive(PlayerSendingVoiceMessageEventArgs ev, Player receiver, bool validate, bool spectators)
    {
        var allow = (!validate || receiver.VoiceModule?.ValidateReceive(ev.Player.ReferenceHub, VoiceChatChannel.Proximity) != VoiceChatChannel.None)
                    && (spectators || receiver.Role != RoleTypeId.Spectator);
        ProximityChatEvents.OnReceiving(ev.Player, receiver, ref allow);
        return allow;
    }

}
