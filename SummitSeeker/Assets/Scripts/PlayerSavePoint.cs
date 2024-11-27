using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavePoint : MonoBehaviour
{
    public Transform activeSavePoint { get; private set;}
    public KeyCode resetToActiveSavePoint = KeyCode.R;

    public void setActiveSavePoint(Transform savePoint) {
        activeSavePoint = savePoint;
    }

// add comment to test workflow run
    private void FixedUpdate() {
        if (activeSavePoint != null && Input.GetKey(resetToActiveSavePoint)) {
            GetComponent<Rigidbody>().position = activeSavePoint.position;
            activeSavePoint = null;
            print("test");
        }
    }
}
