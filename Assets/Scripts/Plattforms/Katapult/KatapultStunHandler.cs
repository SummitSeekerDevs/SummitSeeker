using UnityEngine;

public class KatapultStunHandler
{
    private RigidbodyConstraints playerRbDefaultconstraints;

    public void ToggleFreezePlayerPosition(bool freeze, Rigidbody playerRb)
    {
        if (freeze)
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
        else
            playerRb.constraints = playerRbDefaultconstraints;
    }
}
