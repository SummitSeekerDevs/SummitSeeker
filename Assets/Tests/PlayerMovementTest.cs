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
    private GameObject playerGameobject;
    private PlayerMovementVid playerMovementVid;

    [SetUp]
    public void SetUp()
    {
        gameManager = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
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
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
        GameObject.Destroy(gameManager);
        GameObject.Destroy(playerGameobject);
    }

    /* Aufteilung in Unittestbereich --> funktionen
    und Integrationstestbereich --> tasteneingabe (space == onJump, WASD == onMove ect.)*/

    [UnityTest]
    public IEnumerator PlayerJumpTest()
    {
        yield return new WaitForSeconds(0.5f);

        var playerYGrounded = playerGameobject.transform.position.y;

        // Tasteneingabe hier entfernen und in Integrationstest wie oben beschrieben auslagern
        // Sprung mit Keyboard auslösen
        Press(keyboard[Key.Space]);

        yield return new WaitForSeconds(0.5f); // Warten das er Höhe erreicht

        // Sprungtaste lösen
        Release(keyboard[Key.Space]);

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
