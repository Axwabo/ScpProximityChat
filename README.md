# ScpProximityChat

A LabAPI plugin for SCP: Secret Laboratory that allows SCPs to talk to humans by playing voice messages through SpeakerToys.

`ScpProximityChat.Core` includes minimal functionality, but can be extended by other plugins.
`ScpProximityChat.SecretAPI` is such an extension that
adds Server-Specific Settings to toggle and personalize SCP Proximity Chat.

With the SecretAPI extension, players can assign a keybind to toggle SCP Proximity Chat,
and they can control the volume of other players' Proximity Chat on their client.

> [!IMPORTANT]
> ScpProximityChat depends on [SecretLabNAudio.Core](https://github.com/Axwabo/SecretLabNAudio)
> and optionally [SecretAPI](https://github.com/Misfiy/SecretAPI)

> [!TIP]
> See also: [configuration](#configuration) and [development](#development)

# Installation

## Core

> [!NOTE]
> `SecretLabNAudio.Core` is required to enable SpeakerToy pooling and personalization.
> You don't need modules or dependencies of `SecretLabNAudio` - only the core is required, not even `NAudio.Core`
>
> If you've already installed the full SecretLabNAudio plugin, you can skip the first two steps.

1. Download the `SecretLabNAudio.Core.dll` file from the
   [SecretLabNAudio releases page](https://github.com/Axwabo/SecretLabNAudio/releases)
2. Place the file in the **dependencies** directory:
    - Linux: `.config/SCP Secret Laboratory/LabAPI/dependencies/<port>/`
    - Windows: `%appdata%/SCP Secret Laboratory/LabAPI/dependencies/<port>/`
3. Download the `ScpProximityChat.Core.dll` file from the [releases page](https://github.com/Axwabo/ScpProximityChat/releases)
4. Place the file in the **plugins** directory
    - Linux: `.config/SCP Secret Laboratory/LabAPI/plugins/<port>/`
    - Windows: `%appdata%/SCP Secret Laboratory/LabAPI/plugins/<port>/`
5. Restart the server

## SSSS

0. Install [ScpProximityChat.Core](#core)
1. Download the `SecretAPI.dll` file from the [SecretAPI releases page](https://github.com/Misfiy/SecretAPI/releases)
2. Download the `ScpProximityChat.SecretAPI.dll` from the [releases page](https://github.com/Axwabo/ScpProximityChat/releases)
3. Place both DLLs in the **plugins** directory
4. Restart the server

# Configuration

## Core Config

> [!TIP]
> See also: [SSSS Config](#ssss-config)

### `ShowToggledHint`

If enabled, shows a simple hint when a player enables/disables SCP Proximity Chat.

Disable this setting if the server uses a hint framework (e.g. RueI).

### `Enabled`

The hint to show when a player enables Proximity Chat.

### `Disabled`

The hint to show when a player disables Proximity Chat.

### `AudioSettings`

Defines the default cofiguration of `SpeakerToys` used by SCP Proximity Chat.

`Volume` sets the base volume for speakers. It's a scalar value, 0 = 0%, 1 = 100%.

> [!CAUTION]
> While the volume may be increased to values greater than 1, don't cause earrape to your players!

> [!NOTE]
> SpeakerToys are slightly quieter than the base-game voice chat.
> A value around 1.5 should make them roughly equal.

### `AllowedRoles`

Null, or a list of role types that are allowed to use SCP Proximity Chat.

If null (`allowed_roles: `), the condition will be all SCPs except SCP-079 and SCP-3114.

To customize roles, specify a list similar to the following:

```yaml
allowed_roles:
  - Scp173
  - ClassD
  - Scp106
  - NtfSpecialist
  - Scp049
  - Scientist
  - Scp079
  - ChaosConscript
  - Scp096
  - Scp0492
  - NtfSergeant
  - NtfCaptain
  - NtfPrivate
  - Tutorial
  - FacilityGuard
  - Scp939
  - ChaosRifleman
  - ChaosMarauder
  - ChaosRepressor
  - Scp3114
  - Flamingo
  - AlphaFlamingo
  - ZombieFlamingo
```

### `ValidateReceive`

Set to `true` to enable base-game receiving checks.

Might mess with AudioSettings, but can counter cheaters.

### `AudibleToSpectators`

Set to `false` to prevent spectators from hearing Proximity Chat.

This does not affect `Overwatch` roles.

## SSSS Config

### `ShowAvailableHint`

If enabled, shows a hint with the assigned keybind when SCP Proximity Chat is available.

Disable this setting if the server uses a hint framework (e.g. RueI).

### `AvailableHint`

The hint to show when Proximity Chat is available.
Include `{0}` where the keybind should be placed.

### `Shared`

If enabled, the Server-Specific Settings will be shared across servers under the same account ID.

Generally, this means joining another server hosted on same IP address will use the same settings if already configured
on a different server with the same IP or account ID.

## `Personalization`

If enabled, players will be able to personalize other players' SCP Proximity Chat volume.

This means that Player A can set Player B to be at 50% volume on their end, while Player C might keep Player B on 100%.

The `Master Volume` setting will always be available regardless of this configuration value.

# Development

Reference the `ScpProximityChat.Core.dll` file from the [releases page](https://github.com/Axwabo/ScpProximityChat/releases)

It's encouraged to download the `xml` documentation to help clarify what things do.

## Methods

To define custom conditions that must be met to use SCP Proximity Chat, use the `ProximityChatState.Conditions` list.
It may already include some conditions, clear it to make sure you only have your conditions.

To check whether a player may use SCP Proximity Chat, call the `CanUseProximityChat` method
(located in `ScpProximityChat.Core.ProximityChatState`).

> [!NOTE]
> All conditions in the list must be met for `CanUseProximityChat` to return true.

The `IsProximityChatEnabled` method checks whether the player has SCP Proximity Chat toggled to be enabled.

Use the `ToggleProximityChat` extension method to toggle SCP Proximity Chat for a player.
This does not check conditions beforehand.

## Events

The `ProximityChatEvents` class contains numerous events to help extend functionality.

### `Available`

Invoked when a player's role changes and all conditions are met to use SCP Proximity Chat.

### `Toggled`

Invoked when the Proximity Chat state is toggled with the `ToggleProximityChat` extension method.

### `Sending`

Invoked when the player is about to send a Proximity Chat message.
Set the `allow` parameter to deny sending the message.
The `PlayerSendingVoiceMessageEvent` will still be blocked regardless of allowing this event.

### `Receiving`

Invoked for each player that is about to receive a Proximity Chat message.
Set the `allow` parameter to deny receiving the message.
Canceling the event for one player does not impact other receivers.

### `Personalizing`

Invoked when a player enables SCP Proximity Chat.
If the event has no subscribers, no `SpeakerPersonalization` component will be created.
