using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katapult : MonoBehaviour
{
    Rigidbody playerRb;
    public float shootUpForce = 50f;

    private void OnCollisionEnter(Collision other) {
        playerRb = other.transform.GetComponent<Rigidbody>();
        Invoke(nameof(ShootPlayerUp), 4f);
    }

    // BRAUCHt check ob player noch drauf ist
    private void ShootPlayerUp() {
        
        playerRb.AddForce(transform.up * shootUpForce, ForceMode.Impulse);
    }
}
