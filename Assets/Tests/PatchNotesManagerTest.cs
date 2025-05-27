using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

public class PatchNotesManagerTest
{
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
}
