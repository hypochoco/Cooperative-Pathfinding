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

    #endregion

    #region Grid Constructor Functions

    private void Start() {

        // intial variables
        _cellSize = 0.25f;

        // create a grid
        _grid = new RGrid<RGridNode>(_cellSize);

        // list of grid object positions
        List<Vector3> testList = new List<Vector3>() {
            Vector3.one,
            Vector3.zero
        };

        // add to the grid
        foreach (Vector3 position in testList) {

            (int x, int y, int z) = _grid.GetCoord(position);
            RGridNode test = new RGridNode(x, y, z);
            _grid.Add(x, y, z, test);
            
        }

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
