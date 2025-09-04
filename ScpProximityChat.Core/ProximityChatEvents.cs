using SecretLabNAudio.Core.Extensions;

namespace ScpProximityChat.Core;

/// <summary>Events related to SCP Proximity Chat.</summary>
/// <seealso cref="ProximityChatState.Conditions"/>
/// <seealso cref="ProximityChatState.CanUseProximityChat"/>
public static class ProximityChatEvents
{

    public static event ProximityChatAvailable? Available;

    public static event ProximityChatToggled? Toggled;

    public static event ProximityChatMessageReceiving? Receiving;

    public static event ProximityChatPersonalizing? Personalizing;

    internal static void OnAvailable(Player player) => Available?.Invoke(player);

    internal static void OnToggled(Player player, bool enabled) => Toggled?.Invoke(player, enabled);

    internal static void OnReceiving(Player sender, Player target, ref bool allow) => Receiving?.Invoke(sender, target, ref allow);

    internal static void Personalize(Player player, SpeakerToy toy)
    {
        if (Personalizing != null)
            toy.AddPersonalization(personalization => Personalizing.Invoke(player, personalization));
    }

}
