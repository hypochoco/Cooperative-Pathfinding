using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPathfinding {

    #region Pathfinding Variables

    // Constants for Calculations
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    // Pathfinding Variables
    private RGrid<RGridNode> _grid;
    private RResTable _resTable;
    private RHeap<RGridNode> _openList;
    private List<RGridNode> _closedList;

    // Grid Size
    private int _xLength;
    private int _yLength;
    private int _zLength;

    #endregion

    #region Constructor

    public RPathfinding(RGrid<RGridNode> grid) {
        _grid = grid;
        _resTable = new RResTable();

        _xLength = _grid.Array.GetLength(0);
        _yLength = _grid.Array.GetLength(1);
        _zLength = _grid.Array.GetLength(2);
    }

    #endregion

    #region Pathfinding Functions

    // Calculate Distance Cost
    private int CalculateDistanceCost(RGridNode start, RGridNode end) {

        int xDistance = Mathf.Abs(start.x - end.x);
        int yDistance = Mathf.Abs(start.y - end.y);
        int zDistance = Mathf.Abs(start.z - end.z);
        
        int minDistance = Mathf.Min(xDistance, yDistance, zDistance);

        return minDistance * MOVE_DIAGONAL_COST + 
            (xDistance + yDistance + zDistance - 3 * minDistance) *
            MOVE_STRAIGHT_COST;

    }

    // Return RGridNode at given coordinate
    private RGridNode GetNode(int x, int y, int z) {

        return _grid.GetGridItem(x ,y ,z);
    
    }

    private List<RGridNode> GetNeighbors(RGridNode currentNode) {

        int x = currentNode.x;
        int y = currentNode.y;
        int z = currentNode.z;
        List<RGridNode> neighborList = new List<RGridNode>();

        if (x > 0) { // Left Nodes

            if (GetNode(x - 1, y, z) != null)
                neighborList.Add(GetNode(x - 1, y, z));

        }

        // Right Nodes

        // Forward Nodes

        // Backward Nodes

        return neighborList;

    }

    #endregion

}
