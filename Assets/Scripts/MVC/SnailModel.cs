using UnityEngine;

public record SnailModel(Transform Target, float FollowRange)
{
    public Transform Target { get; private set; } = Target;
    public float FollowRange { get; private set; } = FollowRange;
}

