using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class TestRResTable {

    #region Exmaple Tests

    // // A Test behaves as an ordinary method
    // [Test]
    // public void TestRResTableSimplePasses()
    // {
    //     // Use the Assert class to test conditions
    // }

    // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator TestRResTableWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }

    #endregion

    #region Tests

    [Test]
    public void TestRResTableBasicFunctions() {

        // Create a test Agent
        var testRAgent = new RAgent();

        // Create a ResTable
        var testResTable = new RResTable();

        // Check empty
        Assert.AreEqual(testResTable.TravellerCount((0, 0, 1, 1)), 0);
        Assert.AreEqual(testResTable.TravellerCount((1, 1, 1, 1)), 0);

        // Check not empty
        testResTable.AddTraveller((1, 1, 1, 1), testRAgent);
        Assert.AreEqual(testResTable.TravellerCount((0, 0, 1, 1)), 0);
        Assert.AreEqual(testResTable.TravellerCount((1, 1, 1, 1)), 1);

    }

    #endregion

}
