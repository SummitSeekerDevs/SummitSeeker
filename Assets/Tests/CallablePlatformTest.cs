using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CallablePlatformTest
{
    private GameObject gameManager;
    private GameObject platform,
        target,
        parentObj,
        throwTargetObj,
        throwableObj;
    private Rigidbody rb,
        throwableObjRb;
    private CallablePlatform callablePlatform;
    private ThrowTarget throwTarget;

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

        // Create ThrowTarget
        throwTargetObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throwTargetObj.transform.position = new Vector3(3, 0, 0);
        throwTargetObj.transform.localScale = Vector3.one;
        throwTargetObj.transform.parent = parentObj.transform;

        // Add ThrowTarget Script
        throwTarget = throwTargetObj.AddComponent<ThrowTarget>();
        throwTarget.colliderTag = "Throwable";
        throwTarget.connectedPlatform = callablePlatform;

        // Create fake throwable obj
        throwableObj = new GameObject();
        throwableObj.transform.position = new Vector3(10, 0, 0);
        throwableObj.transform.localScale = Vector3.one;
        throwableObj.transform.tag = "Throwable";
        // ThrowableObj Add Box Collider
        if (throwableObj.GetComponent<BoxCollider>() == null)
        {
            throwableObj.AddComponent<BoxCollider>();
        }

        throwableObjRb = throwableObj.AddComponent<Rigidbody>();
        throwableObjRb.useGravity = false;
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.Destroy(platform);
        GameObject.Destroy(target);
        GameObject.Destroy(throwTargetObj);
        GameObject.Destroy(parentObj);
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

        // Callable Platform deaktivieren
        callablePlatform.moveToPosition = false;
    }

    [UnityTest]
    public IEnumerator ThrowTargetCollisionAktivatesCallablePlatformTest()
    {
        // Callable Platform deaktivieren
        callablePlatform.moveToPosition = false;

        // ThrowableObj und throwTargetObj kollidieren lassen
        throwableObjRb.position = throwTargetObj.transform.position;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            true,
            callablePlatform.moveToPosition,
            "Check turned on platform movement when ThrowTarget is hit"
        );

        Assert.AreEqual(
            false,
            throwTarget.gameObject.activeSelf,
            "Check ThrowTargetObj is inaktive after collision"
        );
    }
}
