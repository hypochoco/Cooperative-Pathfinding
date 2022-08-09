using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Construct the grid in the world 
public class RGridConstructor : MonoBehaviour {
    





    // Physics.CheckSphere...






    #region Grid Constructor Variables
    
    // Grid Constructor Variables
    private float _cellSize;
    private RGrid<RGridNode> _grid; 

    // Getters and Setters
    public RGrid<RGridNode> Grid {
        get { return _grid; }
        private set {}
    }

    #endregion

    #region Grid Constructor Functions

    private void Start() {

        // intial variables
        _cellSize = 0.5f;

        // create a grid
        _grid = new RGrid<RGridNode>(_cellSize);

        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                RGridNode test = new RGridNode(i, 0, j);
                _grid.Add(i, 0, j, test);
            }
        }

        // // Manual Grid Additions
        RGridNode test0 = new RGridNode(10, 1, 5);
        _grid.Add(test0.x, test0.y, test0.z, test0);

        // // Manual Grid Additions
        // RGridNode test0 = new RGridNode(5, 1, 5);
        // _grid.Add(test0.x, test0.y, test0.z, test0);

        // Manual Grid Additions
        // RGridNode test0 = new RGridNode(10, 0, 0);
        // _grid.Add(test0.x, test0.y, test0.z, test0);

        
        // Debug.Log(_grid.GetGridItem(5, 1, 5));

        // Debug.Log(_grid.Array.GetLength(0));
        // Debug.Log(_grid.Array.GetLength(1));
        // Debug.Log(_grid.Array.GetLength(2));

    }

    #endregion

    #region Debug

    // Gizmos
    public void OnDrawGizmos() {

        // Prevent null errors
        if (_grid == null || _grid.Array == null)
            return;

        // Draw Variables
        Gizmos.color = new Color(1, 1, 1, 0.75f);
        float s = _cellSize * 0.75f;

        // Loop through grid to draw objects
        foreach (RGridNode node in _grid.Array) {
            
            // Null check
            if (node == null)
                continue;

            // Convert 
            Vector3 pos = _grid.GetWorld(node.x, node.y, node.z);

            // Draw cube at each point
            Gizmos.DrawCube(pos, new Vector3(s, s, s));
        }
    }

    #endregion

}
