using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class PlayerSafePointTest : InputTestFixture
{
    private GameObject playerGameobject;
    private PlayerSavePoint playerSavePointscript;
    private Keyboard keyboard;
    private PlayerInput_Actions inputActions;
    private GameObject gameManager;

    [SetUp]
    public void SetUp() {

        gameManager = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
        //gameManager.GetComponent<GameManager>();

        inputActions = new PlayerInput_Actions();
        inputActions.Enable(); 
        
        keyboard = InputSystem.AddDevice<Keyboard>();

        // Einrichten des Testspielers mit dem PlayerSavePoint-Script
        playerGameobject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), Vector3.zero, Quaternion.identity);
        //playerGameobject.AddComponent<Rigidbody>();
        playerSavePointscript = playerGameobject.GetComponent<PlayerSavePoint>();
     }
 
     [TearDown]
    public void Teardown() {
        GameObject.Destroy(gameManager);
        GameObject.Destroy(playerGameobject);
    }
    
    [Test]
    public void setActiveSavePoint_UpdatesActiveSavePoint() {
        // Arrange
        var savePoint = new GameObject().transform;

        // Act
        playerSavePointscript.setActiveSavePoint(savePoint);

        // Assert
        Assert.AreEqual(savePoint, playerSavePointscript.activeSavePoint);
    }

    [UnityTest]
    public IEnumerator FixedUpdate_ResetsPositionToActiveSavePoint_WhenKeyIsPressed() {
        // Arrange
        var savePoint = new GameObject().transform;
        savePoint.position = new Vector3(100, 0, 0);
 
        // Gravitation für Rb ausschalten da hier irrelevant
        var rb = playerGameobject.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        playerSavePointscript.setActiveSavePoint(savePoint);

        // Simuliere das Drücken der Taste
        Press(keyboard[Key.R]);
        
        // yield return null is to skip a frame but only use it in edit mode
        yield return new WaitForSeconds(0.1f);

        // Assert
        Assert.AreEqual(savePoint.position, rb.position);

        
    }
}