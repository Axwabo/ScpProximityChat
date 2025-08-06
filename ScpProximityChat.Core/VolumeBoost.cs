using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace ScpProximityChat.Core;

using AudioProcessor = (OpusDecoder Decoder, OpusEncoder Encoder);

public static class VolumeBoost
{

    private static readonly float[] Samples = new float[VoiceChatSettings.PacketSizePerChannel];
    private static readonly byte[] Encoded = new byte[VoiceChatSettings.MaxEncodedSize];

    private static readonly Dictionary<Player, AudioProcessor> AudioProcessors = [];

    public static float Amount { get; set; }

    public static AudioMessage Convert(Player sender, VoiceMessage message)
    {
        if (Mathf.Approximately(Amount, 0))
            return new AudioMessage(0, message.Data, message.DataLength);
        var (decoder, encoder) = GetOrAdd(sender);
        var decoded = decoder.Decode(message.Data, message.DataLength, Samples);
        for (var i = 0; i < decoded; i++)
            Samples[i] *= Amount;
        var encoded = encoder.Encode(Samples, Encoded, decoded);
        return new AudioMessage(0, Encoded, encoded);
    }

    public static AudioProcessor GetOrAdd(Player player)
        => AudioProcessors.TryGetValue(player, out var processors)
            ? processors
            : AudioProcessors[player] = (new OpusDecoder(), new OpusEncoder(OpusApplicationType.Voip));

    public static void Remove(Player player)
    {
        if (!AudioProcessors.Remove(player, out var processors))
            return;
        processors.Decoder.Dispose();
        processors.Encoder.Dispose();
    }

}
