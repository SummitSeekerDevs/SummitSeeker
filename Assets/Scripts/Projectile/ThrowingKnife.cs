using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[assembly: InternalsVisibleTo("PlaymodeTests")]

public class ThrowingKnife : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    internal Transform cam;

    [SerializeField]
    internal Transform attackPoint;

    [SerializeField]
    internal GameObject objectToThrow;

    [Header("Settings")]
    [SerializeField]
    internal int totalThrows = 50;

    [SerializeField]
    private float throwCooldown = 0.1f;

    [Header("Throwing")]
    [SerializeField]
    private float throwForce = 4;

    [SerializeField]
    private float throwUpwardForce = 0f;

    internal bool readyToThrow = true;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<ThrowProjectileSignal>(OnThrow);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<ThrowProjectileSignal>(OnThrow);
    }

    private void OnThrow()
    {
        if (readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    internal void Throw()
    {
        readyToThrow = false;

        Rigidbody projectileRb = CreateProjectile();
        Vector3 forceToAdd = CalculateThrowForce();
        ApplyForceToProjectile(projectileRb, forceToAdd);

        ApplyRotationTorque(projectileRb);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private Rigidbody CreateProjectile()
    {
        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.angularDamping = 0f;

        return rb;
    }

    private Vector3 CalculateThrowForce()
    {
        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // return force
        return forceDirection * throwForce + transform.up * throwUpwardForce;
    }

    private void ApplyForceToProjectile(Rigidbody rb, Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void ApplyRotationTorque(Rigidbody rb)
    {
        Vector3 torque = attackPoint.transform.right * 1000 * Time.deltaTime;
        rb.AddTorque(torque, ForceMode.Impulse);
    }

    private void ResetThrow() => readyToThrow = true;
}

public class ThrowProjectileSignal { }
