using LabApi.Features.Wrappers;
using SecretAPI.Features.UserSettings;
using UnityEngine;

namespace ScpProximityChat.SecretAPI;

public sealed class ProximityChatToggle : CustomKeybindSetting
{

    public ProximityChatToggle() : base(null, "Toggle SCP Proximity Chat", KeyCode.LeftAlt, allowSpectatorTrigger: false)
    {
    }

    protected override CustomSetting CreateDuplicate() => new ProximityChatToggle();

    protected override void HandleSettingUpdate(Player player)
    {
        if (player.CanUseProximityChat())
            player.ToggleProximityChat();
    }

    public override CustomHeader Header { get; } = new("Proximity Chat");

}
