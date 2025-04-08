using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SavePointTriggerTest
{
    private GameObject gameManager;
    private GameObject triggerObj,
        throwableObj,
        savePoint,
        playerGameobject;
    private Rigidbody throwableObjRb;
    private SavePointTrigger savePointTriggerScript;
    private PlayerSavePoint playerSavePointScript;

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

        // Create SavePoint
        savePoint = new GameObject();
        savePoint.transform.position = new Vector3(0, 0, 10);

        // Create TriggerObject
        triggerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        triggerObj.transform.position = new Vector3(3, 0, 0);
        triggerObj.transform.localScale = Vector3.one;

        // triggerObj Add Box Collider
        triggerObj.AddComponent<BoxCollider>().isTrigger = true;

        // Add savePointTriggerScript Script
        savePointTriggerScript = triggerObj.AddComponent<SavePointTrigger>();
        savePointTriggerScript.colliderTag = "Throwable";
        savePointTriggerScript.savePoint = savePoint.transform;

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

        // Einrichten des Testspielers
        playerGameobject = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Player"),
            Vector3.zero,
            Quaternion.identity
        );
        playerGameobject.transform.position = Vector3.zero;

        playerSavePointScript = playerGameobject.GetComponent<PlayerSavePoint>();
    }

    [TearDown]
    public void Teardown()
    {
        GameManager.Destroy(gameManager);
        GameObject.Destroy(throwableObj);
        GameObject.Destroy(savePoint);
        GameObject.Destroy(triggerObj);
    }

    [UnityTest]
    public IEnumerator SavePointTriggerCollisionAddsSavePoint()
    {
        Assert.AreEqual(false, savePointTriggerScript.usedSavePoint, "Check savePoint is not used");

        Assert.IsNull(playerSavePointScript.activeSavePoint, "Check Player has no savepoint");

        throwableObjRb.position = triggerObj.transform.position;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            savePoint.transform,
            playerSavePointScript.activeSavePoint,
            "Check player has savepoint"
        );

        Assert.AreEqual(true, savePointTriggerScript.usedSavePoint, "Check savePoint is used");
    }
}
