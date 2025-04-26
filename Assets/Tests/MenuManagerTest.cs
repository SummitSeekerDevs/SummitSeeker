using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuManagerTest
{
    private GameObject menuManager;

    private int MENU_FPS = 30,
        MENU_VSYNC_MODE = 0;

    private void setGameManager()
    {
        if (MenuManager.Instance != null)
        {
            menuManager = MenuManager.Instance.gameObject;
        }
        else
        {
            menuManager = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MenuManager"));
        }
    }

    [SetUp]
    public void SetUp()
    {
        setGameManager();
    }

    [UnityTest]
    public IEnumerator MenuManagerInitTest()
    {
        yield return null;

        Assert.AreEqual(
            MENU_FPS,
            Application.targetFrameRate,
            "Framerate is set to menu framerate"
        );
        Assert.AreEqual(
            MENU_VSYNC_MODE,
            QualitySettings.vSyncCount,
            "Vsync mode is set to menu framerate"
        );

        Assert.AreEqual(
            MenuState.StartupMenu,
            MenuManager.Instance._currentState,
            "MenuState is startup State"
        );
    }

    [UnityTest]
    public IEnumerator CanUpdateMenuStateWithButtonTest()
    {
        // Startup Menu
        MenuManager.Instance.ButtonUpdateMenuState(0);

        yield return null;

        Assert.AreEqual(MenuState.StartupMenu, MenuManager.Instance._currentState);

        // Options Menu
        MenuManager.Instance.ButtonUpdateMenuState(1);

        yield return null;

        Assert.AreEqual(MenuState.OptionsMenu, MenuManager.Instance._currentState);

        // Credits Menu
        MenuManager.Instance.ButtonUpdateMenuState(2);

        yield return null;

        Assert.AreEqual(MenuState.CreditsMenu, MenuManager.Instance._currentState);
    }
}
