using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SavePointTrigger : MonoBehaviour
{
    [SerializeField]
    internal Transform savePoint;
    private GameObject _playerGameObject;
    internal bool usedSavePoint = false;
    public string colliderTag;

    [Inject]
    public void Construct(GameObject playerGameObject)
    {
        _playerGameObject = playerGameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (
            !usedSavePoint
            && other.transform.CompareTag(colliderTag)
            && _playerGameObject.TryGetComponent<PlayerSavePoint>(
                out PlayerSavePoint playerSavePoint
            )
        )
        {
            Debug.Log("1 use savepoint unlocked");
            playerSavePoint.setActiveSavePoint(savePoint);
            usedSavePoint = true;
        }
    }
}
