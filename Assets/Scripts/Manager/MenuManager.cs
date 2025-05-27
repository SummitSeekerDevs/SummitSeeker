using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[assembly: InternalsVisibleTo("Tests")]

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    internal MenuState _currentState = MenuState.StartupMenu;

    [SerializeField]
    internal GameObject MenuPanel,
        NewsPanel,
        CreditsPanel,
        MainMenuButtons,
        SettingsMenuButtons;

    private void Start()
    {
        InitializeMenuManager();
    }

    private void InitializeMenuManager()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("_instance of MenuManager already exists. Destroying self.");
            Destroy(gameObject);
            return;
        }

        _instance = this;

        // MenuManager darf nur in Hauptmenu Szene existieren
        if (SceneManager.GetActiveScene().name != GameManager.Instance.mainMenuSceneName)
        {
            Debug.LogWarning("MenuManager should not exist in this scene. Destroying self.");
            Destroy(gameObject);
            return;
        }
    }

    private bool IsValidStateTransition(MenuState from, MenuState to)
    {
        // Add state transition validation logic here
        return true;
    }

    public void ButtonUpdateMenuState(int newStateInt)
    {
        if (Enum.IsDefined(typeof(MenuState), newStateInt))
        {
            UpdateMenuState((MenuState)newStateInt);
        }
        else
        {
            Debug.LogError($"Invalid menu state value: {newStateInt}");
        }
    }

    public void UpdateMenuState(MenuState newState)
    {
        if (!IsValidStateTransition(_currentState, newState))
        {
            Debug.LogWarning($"Invalid state transition from {_currentState} to {newState}");
            return;
        }

        _currentState = newState;

        switch (newState)
        {
            case MenuState.StartupMenu:
                HandleStartupMenuState();
                break;
            case MenuState.OptionsMenu:
                HandleOptionsMenuState();
                break;
            case MenuState.CreditsMenu:
                HandleCreditsMenuState();
                break;
            case MenuState.LanguageMenu:
                HandleLanguageMenuState();
                break;
            case MenuState.SoundMenu:
                HandleSoundMenuState();
                break;
            case MenuState.GameplayMenu:
                HandleGameplayMenuState();
                break;
            case MenuState.ControlsMenu:
                HandleControlsMenuState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void HandleStartupMenuState()
    {
        // Panels
        MenuPanel.SetActive(true);
        NewsPanel.SetActive(true);
        CreditsPanel.SetActive(false);

        // Buttons
        MainMenuButtons.SetActive(true);
        SettingsMenuButtons.SetActive(false);
    }

    private void HandleOptionsMenuState()
    {
        // Panels
        MenuPanel.SetActive(true);
        NewsPanel.SetActive(true);
        CreditsPanel.SetActive(false);

        // Buttons
        MainMenuButtons.SetActive(false);
        SettingsMenuButtons.SetActive(true);
    }

    private void HandleCreditsMenuState()
    {
        // Panels
        MenuPanel.SetActive(false);
        NewsPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    // Setting-Views
    private void HandleLanguageMenuState()
    {
        // language menu logic
    }

    private void HandleSoundMenuState()
    {
        // sound menu logic
    }

    private void HandleGameplayMenuState()
    {
        // gameplay menu logic
    }

    private void HandleControlsMenuState()
    {
        // controls menu logic
    }
}

public enum MenuState
{
    StartupMenu = 0,
    OptionsMenu = 1,
    CreditsMenu = 2,
    LanguageMenu = 3,
    SoundMenu = 4,
    GameplayMenu = 5,
    ControlsMenu = 6,
}
