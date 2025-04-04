using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class PlayerMovementTest : InputTestFixture
{
    private GameObject gameManager;
    private Keyboard keyboard;
    private GameObject playerGameobject,
        ground;
    private PlayerMovementVid playerMovementVid;

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

        keyboard = InputSystem.AddDevice<Keyboard>();

        // Einrichten des Testspielers mit dem PlayerSavePoint-Script
        playerGameobject = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Player"),
            Vector3.zero,
            Quaternion.identity
        );

        playerMovementVid = playerGameobject.GetComponent<PlayerMovementVid>();

        // Startposition und "Boden" setzen
        playerGameobject.transform.position = Vector3.zero;
        ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.position = new Vector3(0, -1, 0);
        ground.transform.localScale = new Vector3(10, 1, 10);
        ground.layer = LayerMask.NameToLayer("whatIsGround");

        if (ground.GetComponent<BoxCollider>() == null)
        {
            ground.AddComponent<BoxCollider>();
        }
    }

    [TearDown]
    public void Teardown()
    {
        GameManager.Destroy(gameManager);
        GameObject.Destroy(playerGameobject);
        GameObject.Destroy(ground);
    }

    /* Aufteilung in Unittestbereich --> funktionen
    und Integrationstestbereich --> tasteneingabe (space == onJump, WASD == onMove ect.)*/

    [Test]
    public void PlayerJumpIntegrationsTest()
    {
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.Space]);

        Assert.AreEqual(true, playerMovementVid._jumpingIsPressed, "Jumping");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.Space]);

        Assert.AreEqual(false, playerMovementVid._jumpingIsPressed, "Jumping release");
    }

    [UnityTest]
    public IEnumerator PlayerJumpTest()
    {
        yield return new WaitForSeconds(0.5f);

        var playerYGrounded = playerGameobject.transform.position.y;

        // Tasteneingabe hier entfernen und in Integrationstest wie oben beschrieben auslagern

        // Sprung auslösen
        playerMovementVid.Jump();

        yield return new WaitForSeconds(0.5f); // Warten das er Höhe erreicht

        // Prüfen, ob der Spieler in der Luft ist
        Assert.Greater(
            playerGameobject.transform.position.y,
            playerYGrounded,
            "Spieler sollte in der Luft sein"
        );

        yield return new WaitForSeconds(1f); // Warten bis Spieler wieder landet

        // Prüfen ob er wieder auf dem Boden ist
        Assert.LessOrEqual(
            playerGameobject.transform.position.y,
            playerYGrounded + 0.1f,
            "Spieler sollte wieder auf dem Boden sein"
        );
    }

    [Test]
    public void PlayerMoveIntegrationsTest()
    {
        // Horizontal +
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.D]);
        Assert.AreEqual(1, playerMovementVid.moveInput.x, "Move right");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.D]);
        Assert.AreEqual(0, playerMovementVid.moveInput.x, "Move right release");

        // Horizontal -
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.A]);
        Assert.AreEqual(-1, playerMovementVid.moveInput.x, "Move left");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.A]);
        Assert.AreEqual(0, playerMovementVid.moveInput.x, "Move left release");

        // Vertical +
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.W]);
        Assert.AreEqual(1, playerMovementVid.moveInput.y, "Move forward");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.W]);
        Assert.AreEqual(0, playerMovementVid.moveInput.y, "Move forward release");

        // Vertical -
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.S]);
        Assert.AreEqual(-1, playerMovementVid.moveInput.y, "Move backwards");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.S]);
        Assert.AreEqual(0, playerMovementVid.moveInput.y, "Move backwards release");
    }

    [UnityTest]
    public IEnumerator PlayerMoveOnGroundTest()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 startStepPos = playerGameobject.transform.position;

        // horizontal movement

        // horizontal +
        playerMovementVid.MovePlayer(1f, 0);
        yield return new WaitForSeconds(0.25f);
        // Sollte man hier eine genaue Zahl nehmen oder reicht größer, weil man daran ja sieht das er sich bewegt hat
        Assert.Greater(playerGameobject.transform.position.x, startStepPos.x, "Horizontal +");

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // horizontal -
        playerMovementVid.MovePlayer(-1f, 0);
        yield return new WaitForSeconds(0.25f);
        Assert.Less(playerGameobject.transform.position.x, startStepPos.x, "Horizontal -");

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // vertical movement

        // vertical +
        playerMovementVid.MovePlayer(0, 1f);
        yield return new WaitForSeconds(0.25f);
        Assert.Greater(playerGameobject.transform.position.z, startStepPos.z, "Vertical +");

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // vertical -
        playerMovementVid.MovePlayer(0, -1f);
        yield return new WaitForSeconds(0.25f);
        Assert.Less(playerGameobject.transform.position.z, startStepPos.z, "Vertical -");
    }

    [UnityTest]
    public IEnumerator PlayerMoveInAirTest()
    {
        yield return new WaitForSeconds(0.5f);

        // Sehr hoch in die Luft setzen
        playerGameobject.transform.position = new Vector3(0, 200, 0);

        Vector3 startStepPos = playerGameobject.transform.position;

        // ############# horizontal + ###########
        playerMovementVid.MovePlayer(1f, 0);
        yield return new WaitForSeconds(0.25f);
        Assert.Greater(playerGameobject.transform.position.x, startStepPos.x, "Horizontal +");

        // Sehr hoch in die Luft setzen und Reset velocity
        playerGameobject.transform.position = new Vector3(0, 200, 0);
        playerGameobject.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f); // Rücksetzen auf Physik anwenden

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // ############# horizontal - ###########
        playerMovementVid.MovePlayer(-1f, 0);
        yield return new WaitForSeconds(0.25f);
        Assert.Less(playerGameobject.transform.position.x, startStepPos.x, "Horizontal -");

        // Sehr hoch in die Luft setzen und Reset velocity
        playerGameobject.transform.position = new Vector3(0, 200, 0);
        playerGameobject.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f); // Rücksetzen auf Physik anwenden

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // ############# vertical + ###########
        playerMovementVid.MovePlayer(0, 1f);
        yield return new WaitForSeconds(0.25f);
        Assert.Greater(playerGameobject.transform.position.z, startStepPos.z, "Vertical +");

        // Sehr hoch in die Luft setzen und Reset velocity
        playerGameobject.transform.position = new Vector3(0, 200, 0);
        playerGameobject.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f); // Rücksetzen auf Physik anwenden

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // ############# vertical - ###########
        playerMovementVid.MovePlayer(0, -1f);
        yield return new WaitForSeconds(0.25f);
        Assert.Less(playerGameobject.transform.position.z, startStepPos.z, "Vertical -");
    }

    [Test]
    public void PlayerCrouchIntegrationTest()
    {
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.LeftCtrl]);
        Assert.AreEqual(true, playerMovementVid._crouchingIsPressed, "Crouching");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.LeftCtrl]);
        Assert.AreEqual(false, playerMovementVid._crouchingIsPressed, "Crouching release");
    }
}
