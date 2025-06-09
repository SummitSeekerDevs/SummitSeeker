using UnityEngine;
using Zenject;

public class ThrowingKnife : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform cam;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private GameObject objectToThrow;

    [Header("Settings")]
    [SerializeField]
    private int totalThrows = 50;

    [SerializeField]
    private float throwCooldown = 0.1f;

    [Header("Throwing")]
    [SerializeField]
    private float throwForce = 4;

    [SerializeField]
    private float throwUpwardForce = 0f;

    private bool readyToThrow = true;
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

    private void Throw()
    {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        projectileRb.angularDamping = 0f;

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        // Drehung
        Vector3 torque = attackPoint.transform.right * 1000 * Time.deltaTime;
        projectileRb.AddTorque(torque, ForceMode.Impulse);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow() => readyToThrow = true;
}

public class ThrowProjectileSignal { }
