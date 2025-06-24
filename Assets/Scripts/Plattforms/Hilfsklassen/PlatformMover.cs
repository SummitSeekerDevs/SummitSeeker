using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("EditmodeTests")]

public static class PlatformMover
{
    internal static readonly float THRESHOLD = 0.1f;

    public static Vector3 GetNextPositionTowardsTarget(
        Vector3 current,
        Vector3 target,
        float speed,
        float deltaTime
    )
    {
        return Vector3.MoveTowards(current, target, speed * deltaTime);
    }

    public static bool HasReachedTarget(Vector3 current, Vector3 target)
    {
        return Vector3.Distance(current, target) < THRESHOLD;
    }
}
