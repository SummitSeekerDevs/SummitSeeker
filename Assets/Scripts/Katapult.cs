using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]

public class Katapult : MonoBehaviour
{
    internal Rigidbody playerRb;
    public float shootUpForce = 50f;
    internal RigidbodyConstraints playerRbDefaultconstraints;

    private void OnCollisionEnter(Collision collision)
    {
        playerRb = collision.transform.GetComponent<Rigidbody>();
        playerRbDefaultconstraints = playerRb.constraints;

        ToggleFreezePlayerPosition(true);

        // Set timer
        Invoke(nameof(ShootPlayerUp), 4f);
    }

    internal void ToggleFreezePlayerPosition(bool freeze)
    {
        if (freeze)
        {
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            playerRb.constraints = playerRbDefaultconstraints;
        }
    }

    // BRAUCHt check ob player noch drauf ist
    internal void ShootPlayerUp()
    {
        ToggleFreezePlayerPosition(false);

        playerRb.AddForce(transform.up * shootUpForce, ForceMode.Impulse);

        Debug.Log(playerRb.position);
    }
}
