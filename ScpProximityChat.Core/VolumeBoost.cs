using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace ScpProximityChat.Core;

using AudioProcessor = (OpusDecoder Decoder, OpusEncoder Encoder);

/// <summary>Manages amplification and <see cref="VoiceMessage"/> to <seealso cref="AudioMessage"/> conversion.</summary>
public static class VolumeBoost
{

    private static readonly float[] Samples = new float[VoiceChatSettings.PacketSizePerChannel];
    private static readonly byte[] Encoded = new byte[VoiceChatSettings.MaxEncodedSize];

    private static readonly Dictionary<Player, AudioProcessor> AudioProcessors = [];

    /// <summary>The percentage to boost by. 0 = no amplification, 1 boost = 200% volume.</summary>
    public static float Amount { get; set; }

    /// <summary>Amplifies and converts a <see cref="VoiceMessage"/> to an <seealso cref="AudioMessage"/>.</summary>
    /// <param name="sender">The sender of the message.</param>
    /// <param name="message">The message to convert.</param>
    /// <returns>The converted message.</returns>
    public static AudioMessage Convert(Player sender, VoiceMessage message)
    {
        if (Mathf.Approximately(Amount, 0))
            return new AudioMessage(0, message.Data, message.DataLength);
        var (decoder, encoder) = GetOrAdd(sender);
        var decoded = decoder.Decode(message.Data, message.DataLength, Samples);
        for (var i = 0; i < decoded; i++)
            Samples[i] *= 1 + Amount;
        var encoded = encoder.Encode(Samples, Encoded, decoded);
        return new AudioMessage(0, Encoded, encoded);
    }

    /// <summary>Gets or creates a decoder-encoder pair for the given player.</summary>
    /// <param name="player">The player to create processors for.</param>
    /// <returns>A tuple of <seealso cref="OpusDecoder"/> and <seealso cref="OpusEncoder"/>.</returns>
    public static AudioProcessor GetOrAdd(Player player)
        => AudioProcessors.TryGetValue(player, out var processors)
            ? processors
            : AudioProcessors[player] = (new OpusDecoder(), new OpusEncoder(OpusApplicationType.Voip));

    /// <summary>Removes and disposes the decoder-encoder pair of the given player.</summary>
    /// <param name="player">The player to remove processors for.</param>
    public static void Remove(Player player)
    {
        if (!AudioProcessors.Remove(player, out var processors))
            return;
        processors.Decoder.Dispose();
        processors.Encoder.Dispose();
    }

}
