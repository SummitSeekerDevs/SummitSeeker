using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

public class PatchNotesManagerTest
{
    private GameObject patchnotesManager;
    private PatchNotesManager patchNotesManagerScript;

    [SetUp]
    public void SetUp()
    {
        patchnotesManager = new GameObject("PatchNotesManager");
        patchNotesManagerScript = patchnotesManager.AddComponent<PatchNotesManager>();
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.Destroy(patchnotesManager);
    }

    [Test]
    public void CompareBaseVersionsTest()
    {
        // Versions equal
        int result = PatchNotesManager.CompareBaseVersions("1.2.3", "1.2.3");
        Assert.AreEqual(0, result, "Baseversions are equal");

        // First version greater
        result = PatchNotesManager.CompareBaseVersions("1.2.3", "1.2.0");
        Assert.Greater(result, 0, "First Baseversion is greater");

        // First version smaller
        result = PatchNotesManager.CompareBaseVersions("1.2.0", "1.2.3");
        Assert.Less(result, 0, "First Baseversion is smaller");

        // Should handle different length | First version smaller
        result = PatchNotesManager.CompareBaseVersions("1.2", "1.2.1");
        Assert.Less(result, 0, "First Baseversion is greater");
    }

    [Test]
    public void CompareVersionsWithBuildTest()
    {
        // Compare build number when base is equal
        int result = PatchNotesManager.CompareVersionsWithBuild("1.2.3b4", "1.2.3b5");
        Assert.Less(result, 0, "Compare build number when base is equal");

        // Return base compare when base is different
        result = PatchNotesManager.CompareVersionsWithBuild("1.3.0b1", "1.2.9b10");
        Assert.Greater(result, 0, "Return base compare when base is different");

        // Treat missing build as equal
        result = PatchNotesManager.CompareVersionsWithBuild("1.0.0", "1.0.0");
        Assert.AreEqual(0, result, "Treat missing build as equal");
    }

    [Test]
    public void GetUpdateByVersionTest()
    {
        // Should return correct update when version matches
        var updates = new List<UpdateEntry>
        {
            new UpdateEntry
            {
                version = "1.0.0",
                title = "Test",
                date = "01/01/2025",
                patchnotes = new List<String> { "Fix 1" },
            },
            new UpdateEntry
            {
                version = "1.1.0",
                title = "New",
                date = "04/01/2025",
                patchnotes = new List<String> { "Fix 2" },
            },
        };

        var currentVersion = "1.1.0";

        UpdateEntry result = patchNotesManagerScript.GetUpdateByVersion(updates, currentVersion);

        Assert.IsNotNull(result);
        Assert.AreEqual(currentVersion, result.version);
    }

    [Test]
    public void SetExactUpdateEntryTest_FindEntry()
    {
        TextAsset patchInfofixture = Resources.Load<TextAsset>("Test/Fixture/patchInfo_fixture");
        patchNotesManagerScript.patchNotesFile = patchInfofixture;

        var currentVersion = "0.1.0";

        var resultUpdateEntry = patchNotesManagerScript.SetExactUpdateEntry(currentVersion);

        Assert.IsNotNull(resultUpdateEntry);
        Assert.AreEqual(currentVersion, resultUpdateEntry.version);
        Assert.AreEqual("UPDATE 0.1.0 - TESTNAME 2", resultUpdateEntry.title);
        Assert.AreEqual("25/04/2025", resultUpdateEntry.date);
        Assert.AreEqual("Fixed trampoline bounce bug.", resultUpdateEntry.patchnotes[0]);
    }

    [Test]
    public void SetExactUpdateEntryTest_FileException()
    {
        var currentVersion = "0.1.0";

        void resultGetUpdateEntry() => patchNotesManagerScript.SetExactUpdateEntry(currentVersion);

        Assert.Throws<FileNotFoundException>(resultGetUpdateEntry);
    }
}
