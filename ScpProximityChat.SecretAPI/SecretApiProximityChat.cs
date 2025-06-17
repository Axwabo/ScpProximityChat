using Hints;
using LabApi.Loader.Features.Plugins;
using SecretAPI.Features.UserSettings;

namespace ScpProximityChat.SecretAPI;

public sealed class SecretApiProximityChat : Plugin<SecretApiProximityConfig>
{

    public override string Name => "SSSS Proximity Chat";
    public override string Description => "SSSS-based proximity chat toggle";
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
                "\n\n\n\nSCP Proximity Chat available.\nPress <mark=#77777755><size=0>.</size><space=0.2em><b>{0}</b><space=0.2em><size=0>.</size></mark> to toggle.",
                [new SSKeybindHintParameter(SettingId)],
                HintEffectPresets.FadeInAndOut(0.95f),
                10
            );
    }

}
