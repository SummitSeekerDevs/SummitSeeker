using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]

public class SavePointTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform savePoint;
    private PlayerSavePoint playerSavePoint;
    private bool usedSavePoint = false;
    public string colliderTag;

    private void Start()
    {
        playerSavePoint = GameManager.Instance.playerGO.GetComponent<PlayerSavePoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!usedSavePoint && other.transform.CompareTag(colliderTag))
        {
            Debug.Log("1 use savepoint unlocked");
            playerSavePoint.setActiveSavePoint(savePoint);
            usedSavePoint = true;
        }
    }
}
