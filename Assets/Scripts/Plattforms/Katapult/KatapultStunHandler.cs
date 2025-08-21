using UnityEngine;

public class KatapultStunHandler
{
    private RigidbodyConstraints _playerRbDefaultconstraints;

    public void ToggleFreezePlayerPosition(bool freeze, Rigidbody playerRb)
    {
        if (freeze)
        {
            _playerRbDefaultconstraints = playerRb.constraints;
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
            playerRb.constraints = _playerRbDefaultconstraints;
    }
}
