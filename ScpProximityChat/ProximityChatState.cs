using PlayerRoles.PlayableScps;
using PlayerRoles.PlayableScps.Scp3114;

namespace ScpProximityChat;

public static class ProximityChatState
{

    internal static Dictionary<Player, SpeakerToy> ActiveSpeakers { get; } = [];

    public static bool IsProximityChatEnabled(this Player player) => ActiveSpeakers.ContainsKey(player);

    public static void EnableProximityChat(this Player player)
    {
        if (!player.IsProximityChatEnabled())
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
        if (player.DisableProximityChat())
        {
            ProximityChatEvents.OnToggled(player, false);
            return false;
        }

        player.EnableProximityChat();
        ProximityChatEvents.OnToggled(player, true);
        return true;
    }

    public static bool CanUseProximityChat(this Player player) => player.RoleBase is FpcStandardScp and not Scp3114Role;

}
