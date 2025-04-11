using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]

public class Katapult : MonoBehaviour
{
    Rigidbody playerRb;
    public float shootUpForce = 50f;
    private RigidbodyConstraints playerRbDefaultconstraints;

    private void OnCollisionEnter(Collision collision)
    {
        playerRb = collision.transform.GetComponent<Rigidbody>();
        playerRbDefaultconstraints = playerRb.constraints;

        ToggleFreezePlayerPosition(playerRb, true);

        // Set timer
        Invoke(nameof(shootPlayerUp), 4f);
    }

    internal void ToggleFreezePlayerPosition(Rigidbody playerRb, bool freeze)
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
    internal void shootPlayerUp()
    {
        ToggleFreezePlayerPosition(playerRb, false);

        playerRb.AddForce(transform.up * shootUpForce, ForceMode.Impulse);
    }
}
