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
    private RHeap<RGridNode> _openList;
    private List<RGridNode> _closedList;

    #endregion

    #region Constructor

    // Constructor
    public RPathfinding(RGrid<RGridNode> grid) {
        _grid = grid;
    }

    #endregion

    #region Pathfinding Functions

    // Calculate Distance Cost
    private int CalculateDistanceCost(RGridNode start, RGridNode end) {

        // Distances
        int xDistance = Mathf.Abs(start.x - end.x);
        int yDistance = Mathf.Abs(start.y - end.y);
        int zDistance = Mathf.Abs(start.z - end.z);
        
        // Min distance
        int minDistance = Mathf.Min(xDistance, yDistance, zDistance);

        // Distance between start and end nodes 
        return minDistance * MOVE_DIAGONAL_COST + 
            (xDistance + yDistance + zDistance - 3 * minDistance) *
            MOVE_STRAIGHT_COST;

    }

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
        _openList = new RHeap<RGridNode>(
            _grid.Array.GetLength(0) *
            _grid.Array.GetLength(1) *
            _grid.Array.GetLength(2) 
        );
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
            foreach (RGridNode neighborNode in 
                _grid.GetNeighbors(currentNode.x, currentNode.y, currentNode.z)) {

                // Stop if in the closedList
                if (_closedList.Contains(neighborNode))
                    continue;

                // Stop if not walkable
                if (!neighborNode.Walkable)
                    continue;
                
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
