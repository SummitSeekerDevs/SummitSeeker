using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuManagerTest
{
    private GameObject menuManager,
        uIController;
    private MenuManager menuManagerScript;
    private UIController uIControllerScript;

    [SetUp]
    public void SetUp()
    {
        menuManager = new GameObject("MenuManager");
        menuManagerScript = menuManager.AddComponent<MenuManager>();

        // UIController mit text und panel referenzen erstellen, da kein Mockup
        uIController = new GameObject("UIController");
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(menuManager);
    }

    [Test]
    public void ButtonUpdateMenuStateTest()
    {
        var uIControllerMocked = new Mock<UIController>();
        uIControllerMocked.Setup(mk => mk.ShowStartUpMenu()).Verifiable();

        // Startup Menu
        menuManagerScript.ButtonUpdateMenuState(0);
        Assert.AreEqual(
            MenuState.StartupMenu,
            MenuManager._instance._currentState,
            "Button update state to startup"
        );

        uIControllerMocked.VerifyAll();

        // Options Menu
        /* MenuManager._instance.ButtonUpdateMenuState(1);
         Assert.AreEqual(
             MenuState.OptionsMenu,
             MenuManager._instance._currentState,
             "Button update state to options"
         );
 
         // Credits Menu
         MenuManager._instance.ButtonUpdateMenuState(2);
         Assert.AreEqual(
             MenuState.CreditsMenu,
             MenuManager._instance._currentState,
             "Button update state to credits"
         );*/
    }

    /*[Test]
    public void UpdateMenuStateTest()
    {
        // Startup Menu
        MenuManager._instance.UpdateMenuState(MenuState.StartupMenu);
        Assert.AreEqual(
            MenuState.StartupMenu,
            MenuManager._instance._currentState,
            "Update state to startup"
        );

        // Options Menu
        MenuManager._instance.UpdateMenuState(MenuState.OptionsMenu);
        Assert.AreEqual(
            MenuState.OptionsMenu,
            MenuManager._instance._currentState,
            "Update state to options"
        );

        // Credits Menu
        MenuManager._instance.UpdateMenuState(MenuState.CreditsMenu);
        Assert.AreEqual(
            MenuState.CreditsMenu,
            MenuManager._instance._currentState,
            "Update state to credits"
        );
    }*/
}
