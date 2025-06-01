using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

[assembly: InternalsVisibleTo("Tests")]

public class PatchNotesManager : MonoBehaviour
{
    private static PatchNotesManager _instance;

    [SerializeField]
    internal TextAsset patchNotesFile;

    internal UpdateEntry exactUpdateEntry;

    private void Start()
    {
        InitializePatchNotesManager();
        exactUpdateEntry = SetExactUpdateEntry(Application.version);
        SetUITexts();
    }

    private void SetUITexts()
    {
        UIController._instance.SetVersionUIText();

        if (exactUpdateEntry != null)
        {
            var (title, notes, date) = FormatPatchNotes(exactUpdateEntry);
            UIController._instance.SetPatchNotesUITexts(title, notes, date);
        }
        else
        {
            Debug.Log("No Patchinfo to show. UpdateEntry is null");
        }
    }

    internal UpdateEntry SetExactUpdateEntry(string currentVersion)
    {
        if (patchNotesFile != null)
        {
            // json auslesen
            UpdateList allUpdates = JsonUtility.FromJson<UpdateList>(patchNotesFile.text);

            // Finde neuestes Update
            UpdateEntry exactUpdate = GetUpdateByVersion(allUpdates.updates, currentVersion);

            if (exactUpdate != null)
            {
                return exactUpdate;
            }
            else
            {
                Debug.LogWarning("Kein Patchnotes des Updates für die aktuelle Version gefunden");
                return null;
            }
        }
        else
        {
            throw new FileNotFoundException("Patchnotes file not found");
        }
    }

    private void InitializePatchNotesManager()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("_instance of PatchNotesManager already exists. Destroying self.");
            Destroy(gameObject);
            return;
        }

        _instance = this;

        // PatchNotesManager darf nur in Hauptmenu Szene existieren
        if (SceneManager.GetActiveScene().name != GameManager.Instance.mainMenuSceneName)
        {
            Debug.LogWarning("PatchNotesManager should not exist in this scene. Destroying self.");
            Destroy(gameObject);
            return;
        }
    }

    internal (string title, string notes, string date) FormatPatchNotes(UpdateEntry update)
    {
        string title = update.title + " | PATCH NOTES";
        string notes = "- " + string.Join("\n- ", update.patchnotes);
        string date = update.date;
        return (title, notes, date);
    }

    internal UpdateEntry GetUpdateByVersion(List<UpdateEntry> updates, string targetVersion)
    {
        // Suchen nach einem Update, das exakt der Spielversion entspricht
        foreach (UpdateEntry update in updates)
        {
            if (CompareVersionsWithBuild(update.version, targetVersion) == 0)
            {
                return update; // Exaktes Update gefunden
            }
        }

        return null; // Kein exaktes Update gefunden
    }

    internal UpdateEntry GetLatestUpdate(List<UpdateEntry> updates, string targetVersion)
    {
        // Sortiere Updates nach Versionsnummer
        updates.Sort(
            (updateA, updateB) => CompareVersionsWithBuild(updateB.version, updateA.version)
        );

        // Rückgabe des ersten Updates, das neuer oder gliech der aktuellen Version ist
        foreach (UpdateEntry update in updates)
        {
            if (CompareVersionsWithBuild(update.version, targetVersion) >= 0)
            {
                return update;
            }
        }

        return null; // Kein passendes Update gefunden
    }

    internal static int CompareVersionsWithBuild(string versionA, string versionB)
    {
        var aParts = versionA.Split('b');
        var bParts = versionB.Split('b');

        // Vergleich der Basisversionen ohne Buildnummern
        int baseCompare = CompareBaseVersions(aParts[0], bParts[0]);
        if (baseCompare != 0)
        {
            return baseCompare;
        }

        // Wenn beide Versionen eine Buildnummer haben, vergleiche diese
        if (aParts.Length > 1 && bParts.Length > 1)
        {
            int buildA = int.Parse(aParts[1]);
            int buildB = int.Parse(bParts[1]);
            return buildA.CompareTo(buildB);
        }

        return 0; // Wenn keine Buildnummer vorhanden ist, als gleiche betrachten
    }

    internal static int CompareBaseVersions(string a, string b)
    {
        var aParts = a.Split('.');
        var bParts = b.Split('.');
        int max = Mathf.Max(aParts.Length, bParts.Length);

        for (int i = 0; i < max; i++)
        {
            int aNum = i < aParts.Length ? int.Parse(aParts[i]) : 0;
            int bNum = i < bParts.Length ? int.Parse(bParts[i]) : 0;

            if (aNum > bNum)
                return 1;
            if (aNum < bNum)
                return -1;
        }

        return 0; // Gleich
    }
}

[System.Serializable]
public class UpdateEntry
{
    public string version;
    public string title;
    public string date;
    public List<string> patchnotes;
}

[System.Serializable]
public class UpdateList
{
    public List<UpdateEntry> updates;
}
