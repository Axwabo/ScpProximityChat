namespace ScpProximityChat.SecretAPI.Settings;

internal sealed class ProximityChatToggle : CustomKeybindSetting
{

    public const string Text = "Toggle SCP Proximity Chat";

    private InputMethod? _inputMethod;

    private int _lifeId;
    private uint _cancelSerial;

    public ProximityChatToggle() : base(
        Text.GetStableHashCode(),
        Translation.ToggleLabel,
        KeyCode.LeftAlt,
        true,
        false,
        Translation.ToggleHint
    ) => IsShared = SettingsRegistry.Shared;

    protected override CustomSetting CreateDuplicate() => new ProximityChatToggle();

    protected override void HandleSettingUpdate()
    {
        var owner = KnownOwner!;
        _inputMethod ??= GetPlayerSetting<InputMethod>(SettingsRegistry.Input.Id, owner);
        if (!_inputMethod?.Hold ?? true)
        {
            if (IsPressed && owner.CanUseProximityChat())
                owner.ToggleProximityChat();
            return;
        }

        if (!owner.CanUseProximityChat())
            return;
        if (IsPressed)
        {
            _cancelSerial++;
            if (!owner.IsProximityChatEnabled())
                owner.ToggleProximityChat();
            return;
        }

        _lifeId = owner.LifeId;
        var serial = ++_cancelSerial;
        _ = DisableDelayedAsync(owner, serial);
    }

    public override CustomHeader Header => Headers.ProximityChat;

    private async Awaitable DisableDelayedAsync(Player owner, uint serial)
    {
        var token = owner.ReferenceHub.destroyCancellationToken;
        await Awaitable.WaitForSecondsAsync(0.5f, token);
        if (owner.LifeId != _lifeId || _cancelSerial != serial)
            return;
        if (owner.IsProximityChatEnabled())
            owner.ToggleProximityChat();
        _cancelSerial++;
    }

    public static void InvalidateCancellation(Player player)
    {
        if (TryGetPlayerSetting(player, out ProximityChatToggle? toggle))
            toggle._cancelSerial++;
    }

}
