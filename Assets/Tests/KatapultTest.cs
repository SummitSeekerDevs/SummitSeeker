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
            Vector3.zero,
            Quaternion.identity
        );
        playerObj.transform.position = Vector3.zero;
        playerRb = playerObj.GetComponent<Rigidbody>();
    }

    [UnityTest]
    public IEnumerator ToggleFreezePlayerPositionTest()
    {
        katapultScript.playerRbDefaultconstraints = playerRb.constraints;

        katapultScript.ToggleFreezePlayerPosition(playerRb, true);

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(
            RigidbodyConstraints.FreezeAll,
            playerRb.constraints,
            "Freezed playerRb position and rotation"
        );

        katapultScript.ToggleFreezePlayerPosition(playerRb, false);

        Assert.AreEqual(
            katapultScript.playerRbDefaultconstraints,
            playerRb.constraints,
            "PlayerRb constraints back to default"
        );
    }

    [UnityTest]
    public IEnumerator ShootPlayerUpTest()
    {
        yield return new WaitForSeconds(0.5f);
        katapultScript.playerRbDefaultconstraints = playerRb.constraints;
        katapultScript.playerRb = playerRb;

        Debug.Log(playerRb.position);

        katapultScript.ShootPlayerUp();

        yield return new WaitForSeconds(0.5f);

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
