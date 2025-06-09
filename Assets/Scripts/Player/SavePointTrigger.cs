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
    internal bool _usedSavePoint = false;

    [Inject]
    public void Construct(GameObject playerGameObject)
    {
        _playerGameObject = playerGameObject;
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

        if (_playerGameObject.TryGetComponent<PlayerSavePoint>(out PlayerSavePoint playerSavePoint))
        {
            ActivateSavePoint(playerSavePoint);
        }
    }

    private void ActivateSavePoint(PlayerSavePoint playerSavePoint)
    {
        Debug.Log("1 use savepoint unlocked");
        playerSavePoint.setActiveSavePoint(savePoint);
        _usedSavePoint = true;
    }
    #endregion
}
