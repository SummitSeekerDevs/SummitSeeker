using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class PlayerSafePointTest
{
    private GameObject playerGameobject;
    private PlayerSavePoint playerSavePointscript;
    private Keyboard keyboard;
    private InputTestFixture input;
    private PlayerInput_Actions inputActions;

    [SetUp]
    public void SetUp() {

        input = new InputTestFixture();
        input.Setup();

        inputActions = new PlayerInput_Actions();
        inputActions.Enable(); 
        
        keyboard = InputSystem.AddDevice<Keyboard>();

        // Einrichten des Testspielers mit dem PlayerSavePoint-Script
        playerGameobject = new GameObject();
        playerGameobject.AddComponent<Rigidbody>();
        playerSavePointscript = playerGameobject.AddComponent<PlayerSavePoint>();
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
        savePoint.position = new Vector3(10, 0, 0);

        playerSavePointscript.setActiveSavePoint(savePoint);

        // Simuliere das Drücken der Taste
        //playerSavePointscript.resetToActiveSavePointKey = KeyCode.R;
        //input.Press(keyboard.rKey);
        Key key = (Key)System.Enum.Parse(typeof(Key), 'r'.ToString().ToUpper());
        input.Press(keyboard[key]);
        //input.Release(keyboard.rKey);

        //inputActions.Player.ResetToSavePoint.PerformInteraction();
        // Act
        //yield return new WaitForFixedUpdate();
        yield return null;

        // Assert
        Assert.AreEqual(savePoint.position, playerGameobject.GetComponent<Rigidbody>().position);

        
    }
}