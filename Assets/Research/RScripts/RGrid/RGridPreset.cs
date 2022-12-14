using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RGridPreset
public class RGridPreset {

    #region Constructor

    // Constructor
    public RGridPreset() {}

    #endregion

    #region Presets

    // Select preset
    public RGrid<RGridNode> Preset(int n) {
        switch (n) {
            case 0 :
                return Preset0();
            default :
                return Preset0();
        }
    }

    // Preset 0
    public RGrid<RGridNode> Preset0() {
        
        // intial variables
        float cellSize = 0.5f;
        float cellSizeY = 0.25f;

        // create a grid
        RGrid<RGridNode> grid = new RGrid<RGridNode>(cellSize, cellSizeY);

        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                RGridNode test = new RGridNode(i, 0, j);
                grid.Add(i, 0, j, test);
            }
        }

        // Manual Grid Additions
        RGridNode test0 = new RGridNode(10, 1, 5);
        grid.Add(test0.x, test0.y, test0.z, test0);

        // Manual Grid Additions
        RGridNode test1 = new RGridNode(11, 2, 5);
        grid.Add(test1.x, test1.y, test1.z, test1);

        // Return Preset
        return grid;

    }

    #endregion

}
