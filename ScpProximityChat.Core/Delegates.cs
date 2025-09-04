using SecretLabNAudio.Core;

namespace ScpProximityChat.Core;

/// <summary>A delegate handling when a player may use SCP Proximity Chat on role change.</summary>
/// <param name="player">The player who may now use Proximity Chat.</param>
/// <seealso cref="ProximityChatState.Conditions"/>
public delegate void ProximityChatAvailable(Player player);

/// <summary>A delegate handling when a player toggles SCP Proximity Chat.</summary>
/// <param name="player">The player toggling Proximity Chat.</param>
/// <param name="enabled">Whether Proximity Chat has been enabled or disabled.</param>
public delegate void ProximityChatToggled(Player player, bool enabled);

/// <summary>A delegate handling when a player is about to send a Proximity Chat message.</summary>
/// <param name="sender">The player sending the message.</param>
/// <param name="allow">Whether the message should be processed and sent. Set to false in order to block the message completely.</param>
public delegate void ProximityChatMessageSending(Player sender, ref bool allow);

/// <summary>A delegate handling when a player is about to receive a Proximity Chat message.</summary>
/// <param name="sender">The player sending the message.</param>
/// <param name="target">The player receiving the message.</param>
/// <param name="allow">Whether the message should be sent to the target. Set to false if it shouldn't be sent to the recipient.</param>
public delegate void ProximityChatMessageReceiving(Player sender, Player target, ref bool allow);

/// <summary>A delegate handling when a player's Proximity Chat speaker is being personalized.</summary>
/// <param name="player">The player whose speaker is being personalized.</param>
/// <param name="personalization">The personalization instance that may be configured.</param>
public delegate void ProximityChatPersonalizing(Player player, SpeakerPersonalization personalization);
