using System;
using System.Collections.Generic;
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
}
