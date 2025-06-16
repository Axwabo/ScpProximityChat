using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Wrappers;
using UnityEngine;
using SpeakerToy = AdminToys.SpeakerToy;

namespace ScpProximityChat;

public sealed class PooledSpeaker : MonoBehaviour
{

    private static readonly HashSet<PooledSpeaker> Instances = [];

    public static SpeakerToy Rent(Player player)
    {
        if (Instances.Count == 0)
        {
            var newToy = LabApi.Features.Wrappers.SpeakerToy.Create(player.GameObject.transform);
            newToy.ControllerId = (byte) player.PlayerId;
            return newToy.Base;
        }

        var pooled = Instances.First();
        Instances.Remove(pooled);
        Destroy(pooled);
        pooled.Toy.transform.SetParent(player.GameObject.transform, false);
        pooled.Toy.ControllerId = (byte) player.PlayerId;
        return pooled.Toy;
    }

    public static void Return(SpeakerToy toy) => toy.gameObject.AddComponent<PooledSpeaker>();

    public SpeakerToy Toy { get; private set; } = null!;

    private void Awake()
    {
        Instances.Add(this);
        Toy = GetComponent<SpeakerToy>();
        transform.parent = null;
        gameObject.SetActive(false);
    }

    private void OnDestroy() => Instances.Remove(this);

}
