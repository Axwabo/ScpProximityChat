using System.Collections.Generic;
using LabApi.Features.Wrappers;
using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp3114;
using SpeakerToy = AdminToys.SpeakerToy;

namespace ScpProximityChat;

public static class ProximityChatState
{

    public static Dictionary<Player, SpeakerToy> ActiveSpeakers { get; } = [];

    public static void EnableProximityChat(this Player player)
    {
        if (!ActiveSpeakers.ContainsKey(player))
            ActiveSpeakers.Add(player, PooledSpeaker.Rent(player));
    }

    public static bool DisableProximityChat(this Player player)
    {
        if (!ActiveSpeakers.TryGetValue(player, out var speaker))
            return false;
        PooledSpeaker.Return(speaker);
        ActiveSpeakers.Remove(player);
        return true;
    }

    public static bool ToggleProximityChat(this Player player)
    {
        if (!player.DisableProximityChat())
            player.EnableProximityChat();
        return true;
    }

    public static bool CanUseProximityChat(this Player player) => player.RoleBase is not (FpcStandardScp and not Scp3114Role);

}
