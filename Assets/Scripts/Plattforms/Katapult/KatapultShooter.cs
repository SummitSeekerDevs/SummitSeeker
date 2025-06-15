using UnityEngine;

public class KatapultShooter
{
    public void Shoot(Rigidbody playerRb, float shootUpForce)
    {
        playerRb.AddForce(Vector3.up * shootUpForce, ForceMode.Impulse);
    }
}
