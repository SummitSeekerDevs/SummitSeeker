using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[assembly: InternalsVisibleTo("Tests")]

public class SavePointTrigger : MonoBehaviour
{
    [SerializeField]
    internal Transform savePoint;

    [SerializeField]
    private string colliderTag;

    private GameObject _playerGameObject;
    private SavePointState _savePointState;
    internal bool _usedSavePoint = false;

    [Inject]
    public void Construct(GameObject playerGameObject, SavePointState savePointState)
    {
        _playerGameObject = playerGameObject;
        _savePointState = savePointState;
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckActivateSavePoint(other);
    }

    #region SavePoint activation
    private void CheckActivateSavePoint(Collider other)
    {
        if (_usedSavePoint || !other.transform.CompareTag(colliderTag))
            return;

        ActivateSavePoint();
    }

    private void ActivateSavePoint()
    {
        Debug.Log("1 use savepoint unlocked");
        _savePointState.SetActiveSavePoint(savePoint);
        _usedSavePoint = true;
    }
    #endregion
}
