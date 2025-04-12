using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class KatapultTest
{
    private GameObject gameManager;
    private GameObject katapultObj,
        playerObj;
    private Rigidbody playerRb;
    private Katapult katapultScript;

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
        katapultObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        katapultObj.transform.position = new Vector3(0, -1, 0);
        katapultObj.transform.localScale = new Vector3(30, 1, 30);
        katapultObj.layer = LayerMask.NameToLayer("whatIsGround");

        katapultScript = katapultObj.AddComponent<Katapult>();

        // Einrichten des Testspielers
        playerObj = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Player"),
            new Vector3(0, 1, 0),
            Quaternion.identity
        );
        playerObj.transform.position = Vector3.zero;
        playerRb = playerObj.GetComponent<Rigidbody>();
    }

    [TearDown]
    public void Teardown()
    {
        GameManager.Destroy(gameManager);
        GameObject.Destroy(katapultObj);
        GameObject.Destroy(playerObj);
    }

    [UnityTest]
    public IEnumerator ToggleFreezePlayerPositionTest()
    {
        katapultScript.playerRbDefaultconstraints = playerRb.constraints;

        katapultScript.ToggleFreezePlayerPosition(true);

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(
            RigidbodyConstraints.FreezeAll,
            playerRb.constraints,
            "Freezed playerRb position and rotation"
        );

        katapultScript.ToggleFreezePlayerPosition(false);

        Assert.AreEqual(
            katapultScript.playerRbDefaultconstraints,
            playerRb.constraints,
            "PlayerRb constraints back to default"
        );
    }

    [UnityTest]
    public IEnumerator ShootPlayerUpTest()
    {
        katapultScript.playerRbDefaultconstraints = playerRb.constraints;
        katapultScript.playerRb = playerRb;

        Debug.Log(katapultScript.playerRbDefaultconstraints);

        yield return new WaitForSeconds(0.5f);

        katapultScript.ShootPlayerUp();

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            katapultScript.playerRbDefaultconstraints,
            playerRb.constraints,
            "PlayerRb constraints back to default"
        );

        Vector3 playerRbVelocity = katapultObj.transform.up * katapultScript.shootUpForce;
        Debug.Log(playerRb.position);
        Assert.AreEqual(playerRbVelocity, playerRb.linearVelocity);
    }
}
