using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Path Finding
    // Goal - Handles A* pathfinding algorithm
    // Known Bugs - N/A
    // Todo : N/A
public class PathFinding {

    // Constants for calcualtion
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    // Singleton pattern
    public static PathFinding Instance { get; private set; }
    
    // private Vector3 originPosition;
    private Grid<PathNode> grid;
    private Heap<PathNode> openList;
    private List<PathNode> closedList;

    // PathFinding Constructor 
    // NOTE : originPosition doesn't work, keep at 0
    public PathFinding(int width, int height, float gridSize, Vector3 originPosition) {
        Instance = this;
        grid = new Grid<PathNode>(width, height, gridSize, originPosition,
        (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    // If given position is not on the grid, 
    // finds and returns the nearest walkble PathNode
    public PathNode FindNearestPathNode(Vector3 position) {

        // Hrdcoded fail count
        int loopLimit = 25;
        int loopCount = 0;

        grid.GetXY(position, out int outX, out int outY);

        while ((outX >= grid.GetWidth() || outX < 0 ||
            outY >= grid.GetHeight() || outY < 0)) {

            // Debug.Log("Search Position : " + outX + ", " + outY + 
            //     " | Loop Count : " + loopCount);

            if (loopCount == loopLimit)
                return null;
            loopCount++;

            outX -= (outX >= grid.GetWidth())? 1 : 0;
            outX += (outX < 0)? 1 : 0;
            outY -= (outY >= grid.GetHeight())? 1 : 0;
            outY += (outY < 0)? 1 : 0;
        }
        return GetNode(outX, outY);
    }

    // Finds the nearest PathNode
    public PathNode FindNearestWalkablePathNode(Vector3 position) {

        // Search Limit
        int searchLimit = 25;

        grid.GetXY(position, out int outX, out int outY);   

        // Checks if the initial position is on grid
        if (outX >= grid.GetWidth() || outX < 0 ||
            outY >= grid.GetHeight() || outY < 0) {

            PathNode newPathNode = FindNearestPathNode(position);
            if (newPathNode == null)
                return null;
            
            outX = newPathNode.x;
            outY = newPathNode.y;
        }

        // If already walkable, return this PathNode
        if (GetNode(outX, outY).isWalkable)
            return GetNode(outX, outY);

        // Searches 8 directions 
        int searchX = outX; 
        int searchY = outY;
        for (int i = 0; i < 8; i++) {
            int searchCount = 0;
            searchX = outX;
            searchY = outY;

            Vector2 direction = new Vector2(0, -1) +
                i * new Vector2(Mathf.Cos(i * Mathf.PI / 4),
                Mathf.Sin(i * Mathf.PI / 4));

            while (!GetNode(searchX, searchY).isWalkable && 
                searchCount < searchLimit) {

                searchX += (direction.x >  0.05)?  1 : 0;
                searchX += (direction.x < -0.05)? -1 : 0;
                searchY += (direction.y >  0.05)?  1 : 0;
                searchY += (direction.y < -0.05)? -1 : 0;

                if (searchX >= grid.GetWidth() || searchX < 0 ||
                    searchY >= grid.GetHeight() || searchY < 0)
                    break;

                if (GetNode(searchX, searchY).isWalkable)
                    return GetNode(searchX, searchY);

                searchCount++;
            }
        }
        return null;
    }

    // Find Path (Vector3)
    public List<Vector3> FindPath(Vector3 startWorldPosition, 
    Vector3 endWorldPosition) {

        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        // Attempt to find the nearest PathNodes
        if (startX < 0 || startX >= grid.GetWidth() || 
            startY < 0 || startY >= grid.GetHeight() ||
            (!GetNode(startX, startY).isWalkable)) {
            
            PathNode newStartNode = FindNearestWalkablePathNode(startWorldPosition);
            if (newStartNode == null)
                return null;
            
            startX = newStartNode.x;
            startY = newStartNode.y;
        }

        if (endX < 0 || endX >= grid.GetWidth() ||
            endY < 0 || endY >= grid.GetHeight() ||
            (!GetNode(endX, endY).isWalkable)) {

            PathNode newEndNode = FindNearestWalkablePathNode(endWorldPosition);
            if (newEndNode == null) 
                return null;

            endX = newEndNode.x;
            endY = newEndNode.y;
        }

        // Find path
        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();

            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * 
                grid.GetCellSize() + Vector3.one * grid.GetCellSize() *
                0.5f);
            }
            return vectorPath;
        }
    }

    // Find Path (PathNode)
    public List<PathNode> FindPath(int startX, int startY, 
        int endX, int endY) {

        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        int maxHeapSize = grid.GetWidth() * grid.GetHeight();
        
        openList = new Heap<PathNode>(maxHeapSize);
        closedList = new List<PathNode>();

        openList.Add(startNode);

        // Initialize nodes
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        // Main Loop for Path Calculation
        while(openList.Count > 0) {
            PathNode currentNode = openList.RemoveFirst();

            if (currentNode == endNode) {
                return CalculatePath(endNode);
            }

            closedList.Add(currentNode);

            foreach (PathNode neighborNode in 
            GetNeightborsList(currentNode)) {

                if (closedList.Contains(neighborNode)) {
                    continue;
                }

                if (!neighborNode.isWalkable) {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tenativeGCost = currentNode.gCost + 
                CalculateDistanceCost(currentNode, neighborNode);

                if (tenativeGCost < neighborNode.gCost) {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tenativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode,
                    endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    // Returns a List of Paths
    private List<PathNode> GetNeightborsList(PathNode currentNode) {
        List<PathNode> neighborList = new List<PathNode>();

        // Left Nodes
        if (currentNode.x - 1 >= 0) {

            // Left
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.y));

            // Left Down
            if (currentNode.y - 1 >= 0) {
                neighborList.Add(GetNode(currentNode.x - 1, 
                currentNode.y - 1));
            }

            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) {
                neighborList.Add(GetNode(currentNode.x - 1, 
                currentNode.y + 1));
            }
        }

        // Rigt Nodes
        if (currentNode.x + 1 < grid.GetWidth()) {

            // Right
            neighborList.Add(GetNode(currentNode.x + 1, currentNode.y));

            // Right Down
            if (currentNode.y - 1 >= 0) {
                neighborList.Add(GetNode(currentNode.x + 1, 
                currentNode.y - 1));
            }

            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) {
                neighborList.Add(GetNode(currentNode.x + 1, 
                currentNode.y + 1));
            }
        }

        // Down
        if (currentNode.y - 1 >=0) {
            neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }

        // up
        if (currentNode.y + 1 < grid.GetHeight()) {
            neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighborList;
    }

    // Returns the Grid
    public Grid<PathNode> GetGrid() {
        return grid;
    }

    // Returns the Grid Object at the given Coordinates
    private PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    // Traces Through PathNodes to create a List of Paths
    private List<PathNode> CalculatePath(PathNode endNode) {

        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        path.Add(currentNode);

        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    // Given Two Grid Points, Calculates the Distance Cost
    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + 
        MOVE_STRAIGHT_COST * remaining;
    }
}