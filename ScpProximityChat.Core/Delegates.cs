using SecretLabNAudio.Core;

namespace ScpProximityChat.Core;

public delegate void ProximityChatAvailable(Player player);

public delegate void ProximityChatToggled(Player player, bool enabled);

public delegate void ProximityChatMessageReceiving(Player sender, Player target, ref bool allow);

public delegate void ProximityChatPersonalizing(Player player, SpeakerPersonalization personalization);
