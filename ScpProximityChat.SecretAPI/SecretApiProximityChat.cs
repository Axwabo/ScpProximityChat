using LabApi.Loader.Features.Plugins;
using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI;

public sealed class SecretApiProximityChat : Plugin
{

    public override string Name => "SSSS Proximity Chat";
    public override string Description => "SSSS-based proximity chat toggle";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    public override void Enable() => CustomSetting.Register(new ProximityChatToggle());

    public override void Disable()
    {
    }

}
