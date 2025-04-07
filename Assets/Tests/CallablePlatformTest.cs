using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CallablePlatformTest
{
    private GameObject gameManager;
    private GameObject platform,
        target,
        parentObj;
    private Rigidbody rb;
    private CallablePlatform callablePlatform;

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
        platform.transform.position = new Vector3(0, 5, 0);
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
        target = new GameObject();
        target.transform.position = Vector3.zero;
        target.transform.parent = parentObj.transform;

        // Add MovingPlatform Script
        callablePlatform = platform.AddComponent<CallablePlatform>();
        callablePlatform.moveSpeed = 2f;
        callablePlatform.targetPosition = target.transform;
    }

    [TearDown]
    public void Teardown()
    {
        GameManager.Destroy(gameManager);
        GameObject.Destroy(platform);
    }

    [UnityTest]
    public IEnumerator MovePlatformTowardsTargetPositionTest()
    {
        Vector3 startPosition = rb.position;

        Vector3 nextPosition = callablePlatform.getNextPositionTowardsTarget();

        // Callable Platform aktivieren
        callablePlatform.moveToPosition = true;

        yield return new WaitForFixedUpdate(); // Im FixedUpdate wird die Platform durch rb.MovePosition() zur nextPosition bewegt

        Vector3 actualPosition = rb.position;

        Assert.AreNotEqual(startPosition, actualPosition, "Start and actual position check");
        Assert.Less(
            Vector3.Distance(actualPosition, target.transform.position),
            Vector3.Distance(startPosition, target.transform.position),
            "Distance check"
        );

        Assert.AreEqual(nextPosition, actualPosition, "Next position is actualPosition");

        // Callable Platform deaktivieren
        callablePlatform.moveToPosition = false;
    }

    [UnityTest]
    public IEnumerator TargetPointDistanceCheckTurnOffPlatformMovementTest()
    {
        // Callable Platform aktivieren
        callablePlatform.moveToPosition = true;

        rb.position = new Vector3(
            rb.position.x,
            target.transform.position.y + callablePlatform.distanceToTarget - 0.05f,
            rb.position.z
        );

        // Abwarten das System aktualisierten Position bekommt und Targetaktualisierung in fixedUpdate abwarten
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            false,
            callablePlatform.moveToPosition,
            "Check turned off platform movement"
        );
    }
}
