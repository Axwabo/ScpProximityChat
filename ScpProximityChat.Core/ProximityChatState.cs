using SecretLabNAudio.Core.Pools;

namespace ScpProximityChat.Core;

/// <summary>SCP Proximity Chat extension methods for <see cref="Player"/> wrappers.</summary>
public static class ProximityChatState
{

    /// <summary>All conditions that must be met for a player to be able to use Proximity Chat.</summary>
    public static List<Func<Player, bool>> Conditions { get; } = [];

    internal static Dictionary<Player, SpeakerToy> ActiveSpeakers { get; } = [];

    /// <summary>Gets whether the player has Proximity Chat enabled.</summary>
    /// <param name="player">The player to check.</param>
    /// <returns>If the player has Proximity Chat enabled.</returns>
    /// <seealso cref="CanUseProximityChat"/>
    public static bool IsProximityChatEnabled(this Player player) => ActiveSpeakers.ContainsKey(player);

    /// <summary>Enables Proximity Chat for the player.</summary>
    /// <param name="player">The player to enable Proximity Chat for.</param>
    /// <returns>True if Proximity Chat has been enabled, false if it's already enabled.</returns>
    /// <remarks>This method does not check if the player can use Proximity Chat.</remarks>
    /// <seealso cref="CanUseProximityChat"/>
    /// <seealso cref="DisableProximityChat"/>
    /// <seealso cref="ToggleProximityChat"/>
    public static bool EnableProximityChat(this Player player)
    {
        if (player.IsProximityChatEnabled())
            return false;
        var config = ProximityChatPlugin.Cfg;
        var toy = SpeakerToyPool.Rent(SpeakerToyPool.NextAvailableId, config.AudioSettings, player.GameObject.transform);
        ProximityChatEvents.Personalize(player, toy);
        if (!Mathf.Approximately(config.VolumeBoost, 0))
            VolumeBoost.GetOrAdd(player);
        ActiveSpeakers.Add(player, toy);
        return true;
    }

    /// <summary>Disables Proximity Chat for the player.</summary>
    /// <param name="player">The player to disable Proximity Chat for.</param>
    /// <returns>True if Proximity Chat has been disabled, false if it wasn't enabled.</returns>
    /// <remarks>This method does not check if the player can use Proximity Chat.</remarks>
    /// <seealso cref="CanUseProximityChat"/>
    /// <seealso cref="EnableProximityChat"/>
    /// <seealso cref="ToggleProximityChat"/>
    public static bool DisableProximityChat(this Player player)
    {
        if (!ActiveSpeakers.Remove(player, out var speaker))
            return false;
        SpeakerToyPool.Return(speaker);
        return true;
    }

    /// <summary>Toggles the Proximity Chat state of the player.</summary>
    /// <param name="player">True if Proximity Chat has been enabled, false if it's been disabled.</param>
    /// <returns>Whether Proximity Chat is now enabled.</returns>
    /// <remarks>This method does not check if the player can use Proximity Chat.</remarks
    /// <seealso cref="CanUseProximityChat"/>
    /// <seealso cref="EnableProximityChat"/>
    /// <seealso cref="DisableProximityChat"/>
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

    /// <summary>Gets whether the player may use Proximity Chat.</summary>
    /// <param name="player">The player to check.</param>
    /// <returns>Whether the player meets all conditions to use Proximity Chat.</returns>
    public static bool CanUseProximityChat(this Player player)
    {
        foreach (var condition in Conditions)
            if (!condition(player))
                return false;
        return true;
    }

}
