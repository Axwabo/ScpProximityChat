using Hints;
using LabApi.Loader.Features.Plugins;
using ScpProximityChat.Core;
using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI;

public sealed class SecretApiProximityChat : Plugin<SecretApiProximityConfig>
{

    public override string Name => "SSSS Proximity Chat";
    public override string Description => "SSSS-based proximity chat";
    public override string Author => "Axwabo";
    public override Version Version => GetType().Assembly.GetName().Version;
    public override Version RequiredApiVersion { get; } = new(1, 0, 0);

    public static int SettingId { get; private set; }

    public override void Enable()
    {
        ProximityChatEvents.Available += SendAvailableHint;
        var toggle = new ProximityChatToggle();
        SettingId = toggle.Id;
        CustomSetting.Register(toggle);
    }

    public override void Disable() => ProximityChatEvents.Available -= SendAvailableHint;

    private void SendAvailableHint(Player player)
    {
        if (Config!.ShowAvailableHint)
            player.SendHint(
                Config.AvailableHint,
                [new SSKeybindHintParameter(SettingId)],
                HintEffectPresets.FadeInAndOut(0.95f),
                10
            );
    }

}
