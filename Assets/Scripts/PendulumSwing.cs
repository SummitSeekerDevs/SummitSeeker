using UnityEngine;

public class PendulumSwing : MonoBehaviour
{
    public float speed = 1.5f;
    public float limit = 75f;
    public bool randomStart = false;
    private float startPosition = 0;

    void Awake()
    {
        if (randomStart)
        {
            startPosition = Random.value;
        }
    }

    void Update()
    {
        float angle = limit * Mathf.Sin((Time.time + startPosition) * speed);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
