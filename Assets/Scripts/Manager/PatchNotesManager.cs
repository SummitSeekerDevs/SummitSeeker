using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatchNotesManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset patchNotesFile;

    [SerializeField]
    TMP_Text titleText;

    [SerializeField]
    TMP_Text notesText;

    private void Start()
    {
        if (patchNotesFile != null)
        {
            UpdateList allUpdates = JsonUtility.FromJson<UpdateList>(patchNotesFile.text);

            // Finde neuestes Update
            UpdateEntry exactUpdate = GetUpdateByVersion(allUpdates.updates);

            if (exactUpdate != null)
            {
                DisplayPatchNotes(exactUpdate);
            }
            else
            {
                Debug.LogWarning("Kein Patchnotes des Updates für die aktuelle Version gefunden");
            }
        }
    }

    private void DisplayPatchNotes(UpdateEntry update)
    {
        titleText.text = update.title + " | PATCH NOTES";
        notesText.text = "- " + string.Join("\n- ", update.patchnotes);
    }

    UpdateEntry GetUpdateByVersion(List<UpdateEntry> updates)
    {
        // Suchen nach einem Update, das exakt der Spielversion entspricht
        foreach (UpdateEntry update in updates)
        {
            if (CompareVersionsWithBuild(update.version, Application.version) == 0)
            {
                return update; // Exaktes Update gefunden
            }
        }

        return null; // Kein exaktes Update gefunden
    }

    UpdateEntry GetLatestUpdate(List<UpdateEntry> updates)
    {
        // Sortiere Updates nach Versionsnummer
        updates.Sort(
            (updateA, updateB) => CompareVersionsWithBuild(updateB.version, updateA.version)
        );

        // Rückgabe des ersten Updates, das neuer oder gliech der aktuellen Version ist
        foreach (UpdateEntry update in updates)
        {
            if (CompareVersionsWithBuild(update.version, Application.version) >= 0)
            {
                return update;
            }
        }

        return null; // Kein passendes Update gefunden
    }

    public static int CompareVersionsWithBuild(string versionA, string versionB)
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

    private static int CompareBaseVersions(string a, string b)
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
