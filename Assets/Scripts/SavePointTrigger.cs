using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointTrigger : MonoBehaviour
{
    public Transform savePoint;
    public PlayerSavePoint playerSavePoint;
    private bool usedSavePoint = false;

    private void OnTriggerEnter(Collider other) {
        if (!usedSavePoint) {
            Debug.Log("1 use savepoint unlocked");
            playerSavePoint.SetActiveSavePoint(savePoint);
            usedSavePoint = true;
        }
    }
}
