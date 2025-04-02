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

        Assert.AreEqual(true, playerMovementVid._jumpingIsPressed);

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.Space]);

        Assert.AreEqual(false, playerMovementVid._jumpingIsPressed);
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
}
