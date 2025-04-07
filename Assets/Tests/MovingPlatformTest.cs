using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovingPlatformTest
{
    private GameObject gameManager;
    private GameObject platform,
        point1;
    private Rigidbody rb;
    private MovingPlatform movingPlatform;

    private void setGameManager()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance.gameObject;
        }
        else
        {
            gameManager = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
        }
    }

    [SetUp]
    public void SetUp()
    {
        setGameManager();

        // Create Platform
        platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.transform.position = new Vector3(0, -1, 0);
        platform.transform.localScale = new Vector3(30, 1, 30);
        platform.layer = LayerMask.NameToLayer("whatIsGround");

        // Add Box Collider
        if (platform.GetComponent<BoxCollider>() == null)
        {
            platform.AddComponent<BoxCollider>();
        }

        // Add Rigidbody
        rb = platform.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.mass = 100;
        rb.linearDamping = 0;

        // Create TargetPoint
        point1 = new GameObject();
        point1.transform.position = new Vector3(-2, -1, 0);
        point1.transform.parent = platform.transform;

        // Add MovingPlatform Script
        movingPlatform = platform.AddComponent<MovingPlatform>();
        movingPlatform.moveSpeed = 1.5f;
        Debug.Log(movingPlatform.points);
        movingPlatform.points = new Transform[1];
        movingPlatform.points[0] = point1.transform;
    }

    [TearDown]
    public void Teardown()
    {
        GameManager.Destroy(gameManager);
        GameObject.Destroy(platform);
    }

    [UnityTest]
    public IEnumerator MovePlatformTowardsTargetTest()
    {
        Vector3 startPosition = rb.position;

        Vector3 nextPosition = movingPlatform.getNextPositionTowardsTarget();

        yield return new WaitForFixedUpdate(); // Im FixedUpdate wird die Platform durch rb.MovePosition() zur nextPosition bewegt

        Vector3 actualPosition = rb.position;

        Assert.AreNotEqual(startPosition, actualPosition, "Start and actual position check");
        Assert.Less(
            Vector3.Distance(actualPosition, point1.transform.position),
            Vector3.Distance(startPosition, point1.transform.position),
            "Distance check"
        );

        Assert.AreEqual(nextPosition, actualPosition, "Next position is actualPosition");
    }
}
