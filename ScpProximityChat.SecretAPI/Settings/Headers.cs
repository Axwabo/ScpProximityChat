namespace ScpProximityChat.SecretAPI.Settings;

internal static class Headers
{

    public static CustomHeader ProximityChat { get; private set; } = new("Proximity Chat");

    public static CustomHeader Personalization { get; private set; } = new("Proximity Volume Personalization");

    public static void Reload()
    {
        ProximityChat = new CustomHeader(Translation.ChatHeader);
        Personalization = new CustomHeader(Translation.PersonalizationHeader);
    }

}
