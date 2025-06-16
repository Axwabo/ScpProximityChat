using System.Collections.Generic;
using UnityEngine;

namespace ScpProximityChat;

public sealed class PooledSpeaker : MonoBehaviour
{

    private static readonly HashSet<PooledSpeaker> Instances = [];

    private void Awake() => Instances.Add(this);

    private void OnDestroy() => Instances.Remove(this);

}
