using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovingPlatformTest
{
    private GameObject gameManager;
    private GameObject platform,
        point1,
        point2,
        parentObj;
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

        // Create Parent Object for Moving Platform and Points
        parentObj = new GameObject();

        // Create Platform
        platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.transform.position = new Vector3(0, -1, 0);
        platform.transform.localScale = new Vector3(30, 1, 30);
        platform.layer = LayerMask.NameToLayer("whatIsGround");
        platform.transform.parent = parentObj.transform;

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

        // Create TargetPoint1
        point1 = new GameObject();
        point1.transform.position = new Vector3(-1, -1, 0);
        point1.transform.parent = parentObj.transform;

        // Create TargetPoint2
        point2 = new GameObject();
        point2.transform.position = new Vector3(-0.5f, -1, 0);
        point2.transform.parent = parentObj.transform;

        // Add MovingPlatform Script
        movingPlatform = platform.AddComponent<MovingPlatform>();
        movingPlatform.moveSpeed = 1.5f;
        movingPlatform.points = new Transform[2];
        movingPlatform.points[0] = point1.transform;
        movingPlatform.points[1] = point2.transform;
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

    [UnityTest]
    public IEnumerator TargetPointDistanceCheckSetNextTargetTest()
    {
        rb.position = new Vector3(
            point1.transform.position.x + movingPlatform.distanceToTarget - 0.05f,
            rb.position.y,
            rb.position.z
        );

        // Abwarten das System aktualisierten Position bekommt und Targetaktualisierung in fixedUpdate abwarten
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            movingPlatform.points[1].position,
            movingPlatform.target,
            "Check next targetpoint"
        );

        rb.position = new Vector3(
            point2.transform.position.x - movingPlatform.distanceToTarget + 0.05f,
            rb.position.y,
            rb.position.z
        );

        // Abwarten das System aktualisierten Position bekommt und Targetaktualisierung in fixedUpdate abwarten
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            movingPlatform.points[0].position, // Wenn legth von Points-Array erreicht ist, reset auf Element 0
            movingPlatform.target,
            "Check next targetpoint is first Element"
        );
    }
}
