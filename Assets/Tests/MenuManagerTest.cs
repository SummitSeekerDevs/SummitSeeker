using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MenuManagerTest
{
    private GameObject menuManager,
        gameManager;

    private int MENU_FPS = 30,
        MENU_VSYNC_MODE = 0;

    private void setMenuManager()
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
        setMenuManager();

        // create and set Panels
        MenuManager.Instance.MenuPanel = new GameObject("menuPanel");
        MenuManager.Instance.NewsPanel = new GameObject("newsPanel");
        MenuManager.Instance.CreditsPanel = new GameObject("creditsPanel");
        MenuManager.Instance.MainMenuButtons = new GameObject("mainMenuButtons");
        MenuManager.Instance.SettingsMenuButtons = new GameObject("settingsMenuButtons");
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(MenuManager.Instance.MenuPanel);
        Object.Destroy(MenuManager.Instance.NewsPanel);
        Object.Destroy(MenuManager.Instance.CreditsPanel);
        Object.Destroy(MenuManager.Instance.MainMenuButtons);
        Object.Destroy(MenuManager.Instance.SettingsMenuButtons);

        Object.Destroy(menuManager);
    }

    [UnityTest]
    public IEnumerator MenuManagerInitTest()
    {
        yield return new WaitForEndOfFrame();

        GameManager.Instance.UpdateGameState(GameState.MainMenu);

        yield return new WaitForSeconds(5f);

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

    [Test]
    public void CanUpdateMenuStateWithButtonTest()
    {
        // Startup Menu
        MenuManager.Instance.ButtonUpdateMenuState(0);

        //yield return null;

        Assert.AreEqual(MenuState.StartupMenu, MenuManager.Instance._currentState);

        // Options Menu
        MenuManager.Instance.ButtonUpdateMenuState(1);

        //yield return null;

        Assert.AreEqual(MenuState.OptionsMenu, MenuManager.Instance._currentState);

        // Credits Menu
        MenuManager.Instance.ButtonUpdateMenuState(2);

        //yield return null;

        Assert.AreEqual(MenuState.CreditsMenu, MenuManager.Instance._currentState);
    }
}
