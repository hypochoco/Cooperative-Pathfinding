using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// RGrid
public class RGrid<T> {
    
    #region RGrid Variables

    // Grid Variables
    private float _cellSize;
    private float _cellSizeY;
    private T[,,] _grid;
    private int _count;
    private int _xLength;
    private int _yLength;
    private int _zLength;

    // Getters and Setters
    public float CellSize {
        get { return _cellSize; }
        private set {}
    }
    public float CellSizeY {
        get { return _cellSizeY; }
        private set {}
    }
    public int Count {
        get { return _count; }
        private set {} 
    }
    public T[,,] Array {
        get { return _grid; }
        private set {}
    }

    #endregion

    #region Constructor

    // Constructor
    public RGrid(float cellSize, float cellSizeY) {

        // Initial grid size
        int size = 1;
        _xLength = 1;
        _yLength = 1;
        _zLength = 1;

        // Cell Size
        _cellSize = cellSize;
        _cellSizeY = cellSizeY;

        // Count
        _count = 0;

        // Empty Grid
        _grid = new T[size, size, size];

    }

    #endregion

    #region Grid Functions

    // Resize the underlying array
    public void Resize(int x, int y, int z) {

        // Initial Sizes
        int currentX = _xLength - 1;
        int currentY = _yLength - 1;
        int currentZ = _zLength - 1;

        // Stop if within the size
        if (x <= currentX && y <= currentY && z <= currentZ)
            return;

        // Create new Grid
        T[,,] newGrid = new T[
            (int) Mathf.Max(x, currentX) + 1,
            (int) Mathf.Max(y, currentY) + 1,
            (int) Mathf.Max(z, currentZ) + 1
        ];

        // Update Sizes
        _xLength = newGrid.GetLength(0);
        _yLength = newGrid.GetLength(1);
        _zLength = newGrid.GetLength(2);

        // Transfer all items
        for (int i = 0; i <= currentX; i++) {
            for (int j = 0; j <= currentY; j++) {
                for (int k = 0; k <= currentZ; k++) {
                    newGrid[i, j, k] = _grid[i, j, k];
                }
            }
        }

        // Replace current grid
        _grid = newGrid;
    
    }

    // Add an entry to the grid
    public void Add(int x, int y, int z, T entry) {

        // Ensure boundary conditions
        if (x < 0 || y < 0 || z < 0)
            return;

        // Resize the grid if needed
        Resize(x, y, z);

        // Add the item to the grid
        _grid[x, y, z] = entry;

        // Increase the count
        _count++;

    }

    // Convert world position into a grid position
    public (int, int, int) GetCoord(Vector3 worldPosition) {
        int outX = Mathf.FloorToInt(worldPosition.x / _cellSize);
        int outY = Mathf.FloorToInt(worldPosition.y / _cellSizeY);
        int outZ = Mathf.FloorToInt(worldPosition.z / _cellSize);
        return (outX, outY, outZ);
    }

    // Convert a grid position into world position
    public Vector3 GetWorld(int x, int y, int z) {
        return new Vector3(x * _cellSize, 
            y * _cellSizeY, z * _cellSize);
    }

    // Obtain a grid item
    public T GetGridItem(int x, int y, int z) {
        try {
            return _grid[x, y, z];
        } catch (NullReferenceException) {
            return default(T);
        } catch (IndexOutOfRangeException) {
            return default(T);
        }
    }

    // Find and return list of neighboring nodes
    public List<T> GetNeighbors(int x, int y, int z) {

        // Neighbor List
        List<T> neighborList = new List<T>();

        if (x > 0) { // Left Nodes

            // Left
            if (GetGridItem(x - 1, y, z) != null)
                neighborList.Add(GetGridItem(x - 1, y, z));

            // Left Forward
            if (z + 1 < _zLength && GetGridItem(x - 1, y, z + 1) != null)
                neighborList.Add(GetGridItem(x - 1, y, z + 1));

            // Left Backward
            if (z > 0 && GetGridItem(x - 1, y, z - 1) != null)
                neighborList.Add(GetGridItem(x - 1, y, z - 1));
            
            // Left Up
            if (y + 1 < _yLength && GetGridItem(x - 1, y + 1, z) != null)
                neighborList.Add(GetGridItem(x - 1, y + 1, z));

            // Left Down
            if (y > 0 && GetGridItem(x - 1, y - 1, z) != null)
                neighborList.Add(GetGridItem(x - 1, y - 1, z));

        }

        // Right Nodes
        if (x + 1 < _xLength) {

            // Right
            if (GetGridItem(x + 1, y, z) != null)
                neighborList.Add(GetGridItem(x + 1, y, z));

            // Right Forward
            if (z + 1 < _zLength && GetGridItem(x + 1, y, z + 1) != null)
                neighborList.Add(GetGridItem(x + 1, y, z + 1));

            // Right Backward
            if (z > 0 && GetGridItem(x + 1, y, z - 1) != null)
                neighborList.Add(GetGridItem(x + 1, y, z - 1));
            
            // Right Up
            if (y + 1 < _yLength && GetGridItem(x + 1, y + 1, z) != null)
                neighborList.Add(GetGridItem(x + 1, y + 1, z));

            // Right Down
            if (y > 0 && GetGridItem(x + 1, y - 1, z) != null)
                neighborList.Add(GetGridItem(x + 1, y - 1, z));

        }

        // Forward Nodes
        if (z + 1 < _zLength) {

            // Forward
            if (GetGridItem(x, y, z + 1) != null)
                neighborList.Add(GetGridItem(x, y, z + 1));

            // Foward Up
            if (y + 1 < _yLength && GetGridItem(x, y + 1, z + 1) != null)
                neighborList.Add(GetGridItem(x, y + 1, z + 1));

            // Forward Down
            if (y > 0 && GetGridItem(x, y - 1, z + 1) != null)
                neighborList.Add(GetGridItem(x, y - 1, z + 1));

        }

        // Backward Nodes
        if (z > 0) {

            // Backward
            if (GetGridItem(x, y, z - 1) != null)
                neighborList.Add(GetGridItem(x, y, z - 1));

            // Backward Up
            if (y + 1 < _yLength && GetGridItem(x, y + 1, z - 1) != null)
                neighborList.Add(GetGridItem(x, y + 1, z - 1));

            // Backward Down
            if (y > 0 && GetGridItem(x, y - 1, z - 1) != null)
                neighborList.Add(GetGridItem(x, y - 1, z - 1));

        }

        // Return neighbor list
        return neighborList;

    }

    #endregion

}
