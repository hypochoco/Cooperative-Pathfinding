using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RGrid<T> {

    #region Grid Variables

    // Grid Variables
    private float _cellSize;
    private T[,,] _grid;
    private int _count;

    // Getters and Setters
    public float CellSize {
        get { return _cellSize; }
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
    public RGrid(float cellSize) {

        // Initial grid size
        int size = 1;

        // Cell Size
        _cellSize = cellSize;

        // Count
        _count = 0;

        // Empty Grid
        _grid = new T[size, size, size];

    }

    #endregion

    #region Grid Functions

    // Add an entry to the grid
    public void Add(int x, int y, int z, T entry) {

        // Ensure some conditions
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

        int outX = Mathf.FloorToInt(worldPosition.x / CellSize);
        int outY = Mathf.FloorToInt(worldPosition.y / CellSize);
        int outZ = Mathf.FloorToInt(worldPosition.z / CellSize);

        return (outX, outY, outZ);

    }

    // Convert a grid position into world position
    public Vector3 GetWorld(int x, int y, int z) {

        return new Vector3(x, y, z) * CellSize;

    }

    // Obtain a grid item
    public T GetGridItem(int x, int y, int z) {

        try {

            return _grid[x, y, z];

        } catch (NullReferenceException) {

            return default(T);

        }

    }

    // Resize the underlying array
    public void Resize(int x, int y, int z) {

        // Initial variables
        int currentX = _grid.GetLength(0);
        int currentY = _grid.GetLength(1);
        int currentZ = _grid.GetLength(2);

        // Stop if within the size
        if (x <= currentX && y <= currentY && z <= currentZ)
            return;
        
        // Create new grid with the right size
        T[,,] newGrid = new T[
            (x > currentX)? x : currentX,
            (y > currentX)? y : currentY,
            (z > currentX)? z : currentZ
            ];     
        
        // Transfer all items
        for (int i = 0; i < currentX; i++) {
            for (int j = 0; j < currentY; j++) {
                for (int k = 0; k < currentZ; k++) {
                    newGrid[i, j, k] = _grid[i, j, k];
                }
            }
        }

        // Replace current grid
        _grid = newGrid;
    
    }

    #endregion

}
