using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController _instance { get; private set; }

    #region MainMenu UI-References
    [Header("MainMenu UI References")]
    // - Panels
    [Header("Panels")]
    [SerializeField]
    internal GameObject MenuPanel;

    [SerializeField]
    internal GameObject NewsPanel;

    [SerializeField]
    internal GameObject CreditsPanel;

    // Buttons
    [Header("Buttons")]
    [SerializeField]
    internal GameObject MainMenuButtons;

    [SerializeField]
    internal GameObject SettingsMenuButtons;

    [Header("PatchNotesManager References")]
    [SerializeField]
    internal TMP_Text titleText;

    [SerializeField]
    internal TMP_Text notesText;

    [SerializeField]
    internal TMP_Text dateText;

    [SerializeField]
    internal TMP_Text versionText;

    #endregion

    private void Awake()
    {
        InitializeUIContoller();
    }

    #region Initialize
    private void InitializeUIContoller()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("_instance of UIController already exists. Destroying self.");
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    #endregion

    #region MainMenu - Show menu

    public virtual void ShowStartUpMenu()
    {
        /* if (
             GameManager.Instance.CurrentState == GameState.MainMenu
             && MenuManager._instance._currentState == MenuState.StartupMenu
         )
         {
             // Panels
             MenuPanel.SetActive(true);
             NewsPanel.SetActive(true);
             CreditsPanel.SetActive(false);
 
             // Buttons
             MainMenuButtons.SetActive(true);
             SettingsMenuButtons.SetActive(false);
         }*/
    }

    public void ShowOptionsMenu()
    {
        if (
            GameManager.Instance.CurrentState == GameState.MainMenu
            && MenuManager._instance._currentState == MenuState.OptionsMenu
        )
        {
            // Panels
            MenuPanel.SetActive(true);
            NewsPanel.SetActive(true);
            CreditsPanel.SetActive(false);

            // Buttons
            MainMenuButtons.SetActive(false);
            SettingsMenuButtons.SetActive(true);
        }
    }

    public void ShowCreditsMenu()
    {
        if (
            GameManager.Instance.CurrentState == GameState.MainMenu
            && MenuManager._instance._currentState == MenuState.CreditsMenu
        )
        {
            // Panels
            MenuPanel.SetActive(false);
            NewsPanel.SetActive(false);
            CreditsPanel.SetActive(true);
        }
    }
    #endregion

    #region PatchNotes Texts
    public void SetPatchNotesUITexts(string title, string notes, string date)
    {
        if (GameManager.Instance.CurrentState == GameState.MainMenu)
        {
            titleText.text = title;
            notesText.text = notes;
            dateText.text = date;
        }
    }

    public void SetVersionUIText()
    {
        versionText.text = "Version: " + Application.version;
    }
    #endregion
}
