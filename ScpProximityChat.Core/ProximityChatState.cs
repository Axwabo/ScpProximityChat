using SecretLabNAudio.Core.Extensions;
using SecretLabNAudio.Core.Pools;

namespace ScpProximityChat.Core;

public static class ProximityChatState
{

    public static List<Func<Player, bool>> Conditions { get; } = [];

    internal static Dictionary<Player, SpeakerToy> ActiveSpeakers { get; } = [];

    public static bool IsProximityChatEnabled(this Player player) => ActiveSpeakers.ContainsKey(player);

    public static bool EnableProximityChat(this Player player)
    {
        if (player.IsProximityChatEnabled())
            return false;
        ActiveSpeakers.Add(player, SpeakerToyPool.Rent(player.GameObject.transform)
            .WithId(SpeakerToyPool.NextAvailableId)
            .ApplySettings(ProximityChatPlugin.Cfg.AudioSettings));
        return true;
    }

    public static bool DisableProximityChat(this Player player)
    {
        if (!ActiveSpeakers.TryGetValue(player, out var speaker))
            return false;
        SpeakerToyPool.Return(speaker);
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

    public static bool CanUseProximityChat(this Player player)
    {
        foreach (var condition in Conditions)
            if (!condition(player))
                return false;
        return true;
    }

}
