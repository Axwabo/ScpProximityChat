namespace ScpProximityChat.Core;

public static class ProximityChatEvents
{

    public static event ProximityChatAvailable? Available;

    public static event ProximityChatToggled? Toggled;

    public static event ProximityChatMessageReceiving? Receiving;

    internal static void OnAvailable(Player player) => Available?.Invoke(player);

    internal static void OnToggled(Player player, bool enabled) => Toggled?.Invoke(player, enabled);

    internal static void OnReceiving(Player sender, Player target, ref bool allow) => Receiving?.Invoke(sender, target, ref allow);

}
