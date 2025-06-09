using UnityEngine;

public class SavePointState
{
    public Transform _activeSavePoint { get; private set; }

    public void SetActiveSavePoint(Transform savePoint)
    {
        _activeSavePoint = savePoint;
    }

    public Transform ConsumeSavePoint()
    {
        Transform result = _activeSavePoint;
        _activeSavePoint = null;
        return result;
    }
}
