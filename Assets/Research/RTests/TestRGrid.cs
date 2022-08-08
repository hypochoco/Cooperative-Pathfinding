using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class TestRGrid {

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
    public void TestRGridOutOfBounds() {

        // intial variables
        float _cellSize = 0.25f;

        // create a grid
        var _grid = new RGrid<RGridNode>(_cellSize);

        // list of grid object positions
        List<Vector3> testList = new List<Vector3>() {
            Vector3.one
        };

        // add to the grid
        foreach (Vector3 position in testList) {

            (int x, int y, int z) = _grid.GetCoord(position);
            RGridNode test = new RGridNode(x, y, z);
            _grid.Add(x, y, z, test);
            
        }

        // Assert.AreEqual(2, _grid.Count);
        // Assert.AreEqual("(0, 0, 0)", _grid.GetGridItem(0, 0, 0).ToString());
        // Assert.AreEqual(null, _grid.GetGridItem(1, 1, 1));

    }

    #endregion

}
