using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using PlayerRoles;
using System;
using System.Collections.Concurrent;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;
using Logger = LabApi.Features.Console.Logger;

namespace ScpProximityChat.Core;

internal sealed class EventHandlers : CustomEventsHandler
{
    private static readonly ConcurrentDictionary<uint, (OpusDecoder Decoder, OpusEncoder Encoder)> Codecs = new();
    private static readonly float[] DecodeBuffer = new float[480];
    private static readonly byte[] EncodeBuffer = new byte[512];

    public override void OnPlayerLeft(PlayerLeftEventArgs ev) => ev.Player.DisableProximityChat();

    public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        if (!ev.Player.CanUseProximityChat())
        {
            ev.Player.DisableProximityChat();
            return;
        }

        if (!ev.Player.IsProximityChatEnabled())
            ProximityChatEvents.OnAvailable(ev.Player);
    }

    public override void OnPlayerSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev)
    {
        if (ev.Message.Channel != VoiceChatChannel.ScpChat || !ProximityChatState.ActiveSpeakers.TryGetValue(ev.Player, out var speaker))
            return;
        ev.IsAllowed = false;
        var send = true;
        ProximityChatEvents.OnSending(ev.Player, ref send);
        if (!send)
            return;
        ev.Player.VoiceModule!.CurrentChannel = VoiceChatChannel.Proximity;
        var config = ProximityChatPlugin.Cfg;
        var (data, dataLength) = config.EnableDynamicVolume
            ? AdjustVolume(ev.Player.NetworkId, ev.Message.Data, ev.Message.DataLength, config.AudioSettings.Volume, config.AutoGainMin, config.AutoGainMax)
            : (ev.Message.Data, ev.Message.DataLength);
        var message = new AudioMessage(speaker.ControllerId, data, dataLength);
        var validate = config.ValidateReceive;
        var spectators = config.AudibleToSpectators;
        foreach (var player in Player.ReadyList)
            if (player != ev.Player && AllowReceive(ev, player, validate, spectators))
                player.Connection.Send(message);
    }

    public override void OnServerWaitingForPlayers()
    {
        ProximityChatState.ActiveSpeakers.Clear();
        foreach (var codec in Codecs.Values)
        {
            codec.Decoder.Dispose();
            codec.Encoder.Dispose();
        }
        Codecs.Clear();
    }

    private static bool AllowReceive(PlayerSendingVoiceMessageEventArgs ev, Player receiver, bool validate, bool spectators)
    {
        var allow = (!validate || receiver.VoiceModule?.ValidateReceive(ev.Player.ReferenceHub, VoiceChatChannel.Proximity) != VoiceChatChannel.None)
                    && (spectators || receiver.Role != RoleTypeId.Spectator);
        ProximityChatEvents.OnReceiving(ev.Player, receiver, ref allow);
        return allow;
    }

    private static (OpusDecoder Decoder, OpusEncoder Encoder) GetCodecs(uint playerId)
    {
        return Codecs.GetOrAdd(playerId, _ => (new OpusDecoder(), new OpusEncoder(OpusApplicationType.Voip)));
    }

    private static (byte[] data, int length) AdjustVolume(uint playerId, byte[] source, int length, float globalVolume, float minGain, float maxGain)
    {
        if (length <= 0)
            return (source, length);

        minGain = Math.Max(0f, minGain);
        maxGain = Math.Max(minGain, maxGain);
        globalVolume = Math.Max(0f, globalVolume);

        var (decoder, encoder) = GetCodecs(playerId);

        int sampleCount;
        try
        {
            sampleCount = decoder.Decode(source, length, DecodeBuffer);
        }
        catch (Exception ex)
        {
            Logger.Error($"[ProximityChat] Opus decode failed: {ex.Message}");
            return (source, length);
        }

        if (sampleCount <= 0)
            return (source, length);

        const float targetRms = 0.10f;
        double sumSquares = 0d;
        for (var i = 0; i < sampleCount; i++)
            sumSquares += DecodeBuffer[i] * (double)DecodeBuffer[i];

        var rms = (float)Math.Sqrt(sumSquares / sampleCount);
        var autoGain = rms > 0.0001f ? targetRms / rms : 1f;
        autoGain = Math.Clamp(autoGain, minGain, maxGain);
        var gain = Math.Max(0f, autoGain * globalVolume);

        if (Math.Abs(gain - 1f) < 0.01f)
            return (source, length);

        for (var i = 0; i < sampleCount; i++)
        {
            var sample = DecodeBuffer[i] * gain;
            DecodeBuffer[i] = Math.Clamp(sample, -1f, 1f);
        }

        int encodedLength;
        try
        {
            encodedLength = encoder.Encode(DecodeBuffer, EncodeBuffer, sampleCount);
        }
        catch (Exception ex)
        {
            Logger.Error($"[ProximityChat] Opus encode failed: {ex.Message}");
            return (source, length);
        }

        if (encodedLength <= 0)
            return (source, length);

        var result = new byte[encodedLength];
        Buffer.BlockCopy(EncodeBuffer, 0, result, 0, encodedLength);
        return (result, encodedLength);
    }

}
