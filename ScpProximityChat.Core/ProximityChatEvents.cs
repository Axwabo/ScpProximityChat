using SecretLabNAudio.Core.Extensions;

namespace ScpProximityChat.Core;

/// <summary>Events related to SCP Proximity Chat.</summary>
/// <seealso cref="ProximityChatState.Conditions"/>
/// <seealso cref="ProximityChatState.CanUseProximityChat"/>
public static class ProximityChatEvents
{

    /// <summary>Invoked when a player may use Proximity Chat after a role change.</summary>
    public static event ProximityChatAvailable? Available;

    /// <summary>Invoked when a player toggles Proximity Chat.</summary>
    public static event ProximityChatToggled? Toggled;
    
    /// <summary>Invoked when a player is about to send a Proximity Chat message.</summary>
    public static event ProximityChatMessageSending? Sending;

    /// <summary>Invoked when a player is about to receive a Proximity Chat message.</summary>
    public static event ProximityChatMessageReceiving? Receiving;

    /// <summary>Invoked when a player's Proximity Chat speaker is being personalized.</summary>
    public static event ProximityChatPersonalizing? Personalizing;

    internal static void OnAvailable(Player player) => Available?.Invoke(player);

    internal static void OnToggled(Player player, bool enabled) => Toggled?.Invoke(player, enabled);

    internal static void OnSending(Player player, ref bool allow) => Sending?.Invoke(player, ref allow);

    internal static void OnReceiving(Player sender, Player target, ref bool allow) => Receiving?.Invoke(sender, target, ref allow);

    internal static void Personalize(Player player, SpeakerToy toy)
    {
        if (Personalizing != null)
            toy.AddPersonalization(personalization => Personalizing.Invoke(player, personalization));
    }

}
