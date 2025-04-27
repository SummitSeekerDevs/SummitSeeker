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

        // Einrichten des Testspielers
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
        ground.transform.localScale = new Vector3(30, 1, 30);
        ground.layer = LayerMask.NameToLayer("whatIsGround");
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.Destroy(gameManager);
        GameObject.Destroy(playerGameobject);
        GameObject.Destroy(ground);
    }

    /* Aufteilung in Unittestbereich --> funktionen
    und Integrationstestbereich --> tasteneingabe (space == onJump, WASD == onMove ect.)*/

    // TODO: keyboard Tasten durch Actionmap keybinds ersetzen


    // TODO: Simulieren der Taste waitforSeconds durch waitfornextframe ersetzen, waitforSeconds 0.25f durch waitforfixedupdate ersetzen

    [UnityTest]
    public IEnumerator PlayerJumpIntegrationsTest()
    {
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.Space]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(true, playerMovementVid._jumpingIsPressed, "Jumping");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.Space]);
        yield return new WaitForSeconds(0.1f);
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
        Assert.AreEqual(
            PlayerMovementVid.MovementState.air,
            playerMovementVid.state,
            "MovementState air - jumping"
        );

        yield return new WaitForSeconds(1f); // Warten bis Spieler wieder landet

        // Prüfen ob er wieder auf dem Boden ist
        Assert.LessOrEqual(
            playerGameobject.transform.position.y,
            playerYGrounded + 0.1f,
            "Spieler sollte wieder auf dem Boden sein"
        );
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "MovementState walking - landet"
        );
    }

    [UnityTest]
    public IEnumerator PlayerMoveIntegrationsTest()
    {
        // Horizontal +
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.D]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(1, playerMovementVid.moveInput.x, "Move right");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.D]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, playerMovementVid.moveInput.x, "Move right release");

        // Horizontal -
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.A]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(-1, playerMovementVid.moveInput.x, "Move left");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.A]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, playerMovementVid.moveInput.x, "Move left release");

        // Vertical +
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.W]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(1, playerMovementVid.moveInput.y, "Move forward");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.W]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, playerMovementVid.moveInput.y, "Move forward release");

        // Vertical -
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.S]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(-1, playerMovementVid.moveInput.y, "Move backwards");

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.S]);
        yield return new WaitForSeconds(0.1f);
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
        yield return new WaitForFixedUpdate();
        // Sollte man hier eine genaue Zahl nehmen oder reicht größer, weil man daran ja sieht das er sich bewegt hat
        Assert.Greater(playerGameobject.transform.position.x, startStepPos.x, "Horizontal +");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "MovementState walking - horizontal +"
        );

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // horizontal -
        playerMovementVid.MovePlayer(-1f, 0);
        yield return new WaitForSeconds(0.25f);
        Assert.Less(playerGameobject.transform.position.x, startStepPos.x, "Horizontal -");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "MovementState walking - horizontal -"
        );

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // vertical movement

        // vertical +
        playerMovementVid.MovePlayer(0, 1f);
        yield return new WaitForSeconds(0.25f);
        Assert.Greater(playerGameobject.transform.position.z, startStepPos.z, "Vertical +");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "MovementState walking - vertical +"
        );

        // Step position setzen
        startStepPos = playerGameobject.transform.position;

        // vertical -
        playerMovementVid.MovePlayer(0, -1f);
        yield return new WaitForSeconds(0.25f);
        Assert.Less(playerGameobject.transform.position.z, startStepPos.z, "Vertical -");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "MovementState walking - vertical -"
        );
    }

    [UnityTest]
    public IEnumerator PlayerMoveInAirTest()
    {
        yield return new WaitForSeconds(0.5f);

        // Sehr hoch in die Luft setzen
        playerGameobject.transform.position = new Vector3(0, 200, 0);
        Vector3 startStepPos = playerGameobject.transform.position;
        yield return new WaitForSeconds(0.1f);

        // ############# horizontal + ###########
        playerMovementVid.MovePlayer(1f, 0);
        yield return new WaitForSeconds(0.25f);
        Assert.Greater(playerGameobject.transform.position.x, startStepPos.x, "Horizontal +");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.air,
            playerMovementVid.state,
            "MovementState air - horizontal +"
        );

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
        Assert.AreEqual(
            PlayerMovementVid.MovementState.air,
            playerMovementVid.state,
            "MovementState air - horizontal -"
        );

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
        Assert.AreEqual(
            PlayerMovementVid.MovementState.air,
            playerMovementVid.state,
            "MovementState air - vertical +"
        );

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
        Assert.AreEqual(
            PlayerMovementVid.MovementState.air,
            playerMovementVid.state,
            "MovementState air - vertical -"
        );
    }

    [UnityTest]
    public IEnumerator PlayerCrouchIntegrationTest()
    {
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.LeftCtrl]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(true, playerMovementVid._crouchingIsPressed, "Crouching");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.crouching,
            playerMovementVid.state,
            "Crouching"
        );

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.LeftCtrl]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(false, playerMovementVid._crouchingIsPressed, "Crouching release");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "Crouching release"
        );
    }

    [UnityTest]
    public IEnumerator PlayerCrouchScaleTest()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 normalPlayerScale = playerGameobject.transform.localScale;

        // Crouch
        playerMovementVid.Crouch();

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(
            playerMovementVid.crouchYScale,
            playerGameobject.transform.localScale.y,
            "Crouch scale"
        );

        // Crouch reset
        playerMovementVid.ResetCrouch();

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(
            normalPlayerScale.y,
            playerGameobject.transform.localScale.y,
            "Normal scale (without crouching)"
        );
    }

    [UnityTest]
    public IEnumerator PlayerMoveCrouchWalkSprintAirSpeedTest()
    {
        yield return new WaitForSeconds(0.5f); // Warten das Spieler den Boden erreicht

        // Crouch walk

        // Crouch Taste simulieren und warten das System es erkennt
        Press(keyboard[Key.LeftCtrl]);
        yield return new WaitForSeconds(0.1f);

        Press(keyboard[Key.W]);
        yield return new WaitForSeconds(0.5f); // halbe Sekunde Zeit für Movement

        Assert.AreEqual(
            PlayerMovementVid.MovementState.crouching,
            playerMovementVid.state,
            "MovementState crouching"
        );

        Assert.AreEqual(1f, playerMovementVid.moveInput.y, "Moving forward while crouching");

        Vector3 flatVel = new Vector3(
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.x,
            0f,
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.z
        );

        Assert.AreEqual(playerMovementVid.crouchSpeed, flatVel.magnitude, "Crouch speed check");

        // Tasten loslassen und warten das System es erkennt
        Release(keyboard[Key.W]);
        Release(keyboard[Key.LeftCtrl]);
        yield return new WaitForSeconds(0.1f);

        // Walking

        Press(keyboard[Key.W]);
        yield return new WaitForSeconds(0.5f); // halbe Sekunde Zeit für Movement

        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "MovementState walking"
        );

        Assert.AreEqual(1f, playerMovementVid.moveInput.y, "Moving forward while walking");

        flatVel = new Vector3(
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.x,
            0f,
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.z
        );

        Assert.AreEqual(playerMovementVid.walkSpeed, flatVel.magnitude, "Walk speed check");

        // Tasten loslassen und warten das System es erkennt
        Release(keyboard[Key.W]);
        yield return new WaitForSeconds(0.1f);

        // Sprinting

        // Sprint Taste simulieren und warten das System es erkennt
        Press(keyboard[Key.LeftShift]);
        yield return new WaitForSeconds(0.1f);

        Press(keyboard[Key.W]);
        yield return new WaitForSeconds(0.5f); // halbe Sekunde Zeit für Movement

        Assert.AreEqual(
            PlayerMovementVid.MovementState.sprinting,
            playerMovementVid.state,
            "MovementState sprinting"
        );

        Assert.AreEqual(1f, playerMovementVid.moveInput.y, "Moving forward while sprinting");

        flatVel = new Vector3(
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.x,
            0f,
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.z
        );

        Assert.AreEqual(playerMovementVid.sprintSpeed, flatVel.magnitude, "Sprint speed check");

        // Tasten loslassen und warten das System es erkennt
        Release(keyboard[Key.W]);
        Release(keyboard[Key.LeftShift]);
        yield return new WaitForSeconds(0.1f);

        // Air
        // Sehr hoch in die Luft setzen und Reset velocity
        playerGameobject.transform.position = new Vector3(0, 200, 0);
        playerGameobject.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f); // Rücksetzen auf Physik anwenden

        Press(keyboard[Key.W]);
        yield return new WaitForSeconds(0.5f); // halbe Sekunde Zeit für Movement

        Assert.AreEqual(
            PlayerMovementVid.MovementState.air,
            playerMovementVid.state,
            "MovementState air"
        );

        Assert.AreEqual(1f, playerMovementVid.moveInput.y, "Moving forward while in air");

        flatVel = new Vector3(
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.x,
            0f,
            playerGameobject.GetComponent<Rigidbody>().linearVelocity.z
        );

        Assert.AreEqual(
            playerMovementVid.moveSpeed * playerMovementVid.airMultiplier,
            flatVel.magnitude * playerMovementVid.airMultiplier,
            "Air speed check"
        );

        // Tasten loslassen und warten das System es erkennt
        Release(keyboard[Key.W]);
        Release(keyboard[Key.LeftShift]);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator PlayerSprintIntegrationsTest()
    {
        // Simuliere das Drücken der Taste
        Press(keyboard[Key.LeftShift]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(true, playerMovementVid._sprintingIsPressed, "Sprinting");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.sprinting,
            playerMovementVid.state,
            "To sprinting"
        );

        // Simuliere das Loslassen der Taste
        Release(keyboard[Key.LeftShift]);
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(false, playerMovementVid._sprintingIsPressed, "Sprinting release");
        Assert.AreEqual(
            PlayerMovementVid.MovementState.walking,
            playerMovementVid.state,
            "Sprinting to walking"
        );
    }

    [UnityTest]
    public IEnumerator PlayerFallUnderMapTest()
    {
        // Leeres Spawnpoint Gameobject erstellen und position zuweisen
        GameObject playerSpawnPoint = new GameObject("Spawnpoint");
        playerSpawnPoint.transform.position = new Vector3(7, 7, 7);

        playerMovementVid.spawnPoint = playerSpawnPoint.transform;
        playerGameobject.GetComponent<Rigidbody>().isKinematic = true;

        playerGameobject.GetComponent<Rigidbody>().position = new Vector3(0, -20f, 0);

        playerMovementVid.ResetUnderMap();

        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(
            playerMovementVid.spawnPoint.position,
            playerGameobject.transform.position,
            "Reset under map to spawnpoint"
        );

        playerGameobject.GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Destroy(playerSpawnPoint);
    }

    [UnityTest]
    public IEnumerator PlayerOnSlopeTest()
    {
        ground.transform.eulerAngles = new Vector3(0, 0, -20f);

        yield return new WaitForSeconds(0.5f);

        bool isOnSlope = playerMovementVid.OnSlope();

        Assert.AreEqual(true, isOnSlope, "On Slope");
    }
}
