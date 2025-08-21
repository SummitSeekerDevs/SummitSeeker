using UnityEngine;

public class PlatformPath
{
    private Transform[] _points;
    private int currentIndex = 0;

    public PlatformPath(Transform[] points)
    {
        _points = points;
    }

    public Vector3 GetNextTarget()
    {
        currentIndex++;

        if (currentIndex >= _points.Length)
        {
            currentIndex = 0;
            return _points[currentIndex].position;
        }
        else
        {
            return _points[currentIndex].position;
        }
    }
}
