using UnityEngine;

public class SavePointState
{
    public Transform _activeSavePoint { get; private set; }

    public virtual void SetActiveSavePoint(Transform savePoint)
    {
        _activeSavePoint = savePoint;
    }

    public virtual Transform ConsumeSavePoint()
    {
        Transform result = _activeSavePoint;
        _activeSavePoint = null;
        return result;
    }
}
