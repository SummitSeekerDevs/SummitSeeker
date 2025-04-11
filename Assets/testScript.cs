using Unity.VisualScripting;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public GameObject currentBullet,
        bulletPrefab,
        shootPointLeft;

    private float lastShot,
        fireRate = 4f,
        shootForce = 30f;

    public void Update()
    {
        if (Time.time > lastShot + fireRate)
        {
            currentBullet = Instantiate(
                bulletPrefab,
                shootPointLeft.transform.position,
                Quaternion.identity
            );
            currentBullet
                .GetComponent<Rigidbody>()
                .AddForce(shootPointLeft.transform.forward * shootForce, ForceMode.Impulse);

            lastShot = Time.time;
        }
    }
}
