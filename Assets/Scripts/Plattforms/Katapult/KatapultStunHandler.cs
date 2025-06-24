using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("EditmodeTests")]

public class KatapultStunHandler
{
    internal RigidbodyConstraints _playerRbDefaultconstraints;

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
