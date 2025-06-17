namespace ScpProximityChat.Core;

public static class ProximityChatEvents
{

    public static event Action<Player>? Available;

    public static event Action<Player, bool>? Toggled;

    internal static void OnAvailable(Player player) => Available?.Invoke(player);

    internal static void OnToggled(Player player, bool enabled) => Toggled?.Invoke(player, enabled);

}
