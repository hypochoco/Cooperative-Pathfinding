using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles Pathfinding
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

    // Getters and Setters
    public RResTable ResTable {
        get { return _resTable; }
        private set {}
    }

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

    #region NeighborList

    // Return RGridNode at given coordinate
    private RGridNode GetNode(int x, int y, int z) {

        return _grid.GetGridItem(x ,y ,z);
    
    }

    // Find and return list of neighboring nodes
    private List<RGridNode> GetNeighbors(RGridNode currentNode) {

        int x = currentNode.x;
        int y = currentNode.y;
        int z = currentNode.z;
        List<RGridNode> neighborList = new List<RGridNode>();

        if (x > 0) { // Left Nodes

            // Left
            if (GetNode(x - 1, y, z) != null)
                neighborList.Add(GetNode(x - 1, y, z));

            // Left Forward
            if (z + 1 < _zLength && GetNode(x - 1, y, z + 1) != null)
                neighborList.Add(GetNode(x - 1, y, z + 1));

            // Left Backward
            if (z > 0 && GetNode(x - 1, y, z - 1) != null)
                neighborList.Add(GetNode(x - 1, y, z - 1));
            
            // Left Up
            if (y + 1 < _yLength && GetNode(x - 1, y + 1, z) != null)
                neighborList.Add(GetNode(x - 1, y + 1, z));

            // Left Down
            if (y > 0 && GetNode(x - 1, y - 1, z) != null)
                neighborList.Add(GetNode(x - 1, y - 1, z));

        }

        // Right Nodes
        if (x + 1 < _xLength) {

            // Right
            if (GetNode(x + 1, y, z) != null)
                neighborList.Add(GetNode(x + 1, y, z));

            // Right Forward
            if (z + 1 < _zLength && GetNode(x + 1, y, z + 1) != null)
                neighborList.Add(GetNode(x + 1, y, z + 1));

            // Right Backward
            if (z > 0 && GetNode(x + 1, y, z - 1) != null)
                neighborList.Add(GetNode(x + 1, y, z - 1));
            
            // Right Up
            if (y + 1 < _yLength && GetNode(x + 1, y + 1, z) != null)
                neighborList.Add(GetNode(x + 1, y + 1, z));

            // Right Down
            if (y > 0 && GetNode(x + 1, y - 1, z) != null)
                neighborList.Add(GetNode(x + 1, y - 1, z));

        }

        // Forward Nodes
        if (z + 1 < _zLength) {

            // Forward
            if (GetNode(x, y, z + 1) != null)
                neighborList.Add(GetNode(x, y, z + 1));

            // Foward Up
            if (y + 1 < _yLength && GetNode(x, y + 1, z + 1) != null)
                neighborList.Add(GetNode(x, y + 1, z + 1));

            // Forward Down
            if (y > 0 && GetNode(x, y - 1, z + 1) != null)
                neighborList.Add(GetNode(x, y - 1, z + 1));

        }

        // Backward Nodes
        if (z > 0) {

            // Backward
            if (GetNode(x, y, z - 1) != null)
                neighborList.Add(GetNode(x, y, z - 1));

            // Backward Up
            if (y + 1 < _yLength && GetNode(x, y + 1, z - 1) != null)
                neighborList.Add(GetNode(x, y + 1, z - 1));

            // Backward Down
            if (y > 0 && GetNode(x, y - 1, z - 1) != null)
                neighborList.Add(GetNode(x, y - 1, z - 1));

        }

        return neighborList;

    }

    #endregion

    // Calculate Path
    public List<RGridNode> CalculatePath(RAgent agent, RGridNode endNode) {

        // Path
        List<RGridNode> path = new List<RGridNode>();

        // Current Node
        RGridNode currentNode = endNode;
        path.Add(currentNode);

        // Trace through PreviousNodes until startNode reached
        while (currentNode.PreviousNode != null) {
            path.Insert(0, currentNode.PreviousNode);
            currentNode = currentNode.PreviousNode;
        }

        // Add Travellers
        int i = 0;
        foreach (RGridNode node in path) {
            _resTable.AddTraveller((node.x, node.y, node.z, i), agent);
            i++;
        }
        
        // Return Path
        return path;

    }

    // FindPath
    public List<RGridNode> FindPath(RAgent agent, int startX, int startY, int startZ,
        int endX, int endY, int endZ) {
        
        // Initialize Start and End Nodes
        RGridNode startNode = _grid.GetGridItem(startX, startY, startZ);
        RGridNode endNode = _grid.GetGridItem(endX, endY, endZ);

        // Ensure Start and End Nodes exist
        if (startNode == null || endNode == null)
            return null;
        
        // Open and Closed List for A*
        _openList = new RHeap<RGridNode>(_xLength * _yLength * _zLength);
        _closedList = new List<RGridNode>();

        // Add startNode to openList
        _openList.Add(startNode);

        // Initialize all nodes
        foreach (RGridNode node in _grid.Array) {

            if (node == null)
                continue;

            node.GCost = int.MaxValue;
            node.CalculateFCost();
            node.PreviousNode = null;
        }

        // Initialize Start Node
        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Main Pathfinding Loop
        while (_openList.Count > 0) {
            
            // Find the node with the lowest value
            RGridNode currentNode = _openList.RemoveFirst();

            // Check if endNode reached
            if (currentNode == endNode)
                return CalculatePath(agent, endNode);

            // Add processed node to closedList
            _closedList.Add(currentNode);

            // Iterate through neighboring nodes
            foreach (RGridNode neighborNode in GetNeighbors(currentNode)) {

                // Stop if in the closedList
                if (_closedList.Contains(neighborNode))
                    continue;

                // Stop if not walkable
                if (!neighborNode.Walkable)
                    continue;

                // TODO : Stop if reserved in the table!!!
                
                // Tentative gCost
                int tenativeGCost = currentNode.GCost + 
                    CalculateDistanceCost(currentNode, neighborNode);
                
                // If gCost is lower
                if (tenativeGCost < neighborNode.GCost) {

                    neighborNode.PreviousNode = currentNode;
                    neighborNode.GCost = tenativeGCost;
                    neighborNode.HCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!_openList.Contains(neighborNode))
                        _openList.Add(neighborNode);

                }

            }

        }

        // If path not found
        return null;

    }

    #endregion

}
