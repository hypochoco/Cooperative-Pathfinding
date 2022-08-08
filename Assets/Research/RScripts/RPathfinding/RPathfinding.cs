using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPathfinding {

    #region Pathfinding Variables

    // Constants for Calculations
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    // Pathfinding Variables
    RGrid<RGridNode> _grid;
    RResTable _resTable;

    // - open list (heap)
    // - closed list 

    #endregion

    #region Constructor

    public RPathfinding(RGrid<RGridNode> grid) {
        _grid = grid;
        _resTable = new RResTable();
    }

    #endregion

    #region Pathfinding Functions

    // Pathfinding Functions



    #endregion

}
