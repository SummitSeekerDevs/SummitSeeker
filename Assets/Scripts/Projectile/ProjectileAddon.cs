using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    private Rigidbody _rb;

    private bool targetHit = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;

        targetHit = true;
        Stick(other);
    }

    private void Stick(Collision other)
    {
        // makes sure projectile sticks to surface
        _rb.isKinematic = true;
        // make sure projectile moves with target
        transform.SetParent(other.transform);
    }
}
