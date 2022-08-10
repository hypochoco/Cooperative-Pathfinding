using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Physics.CheckSphere...

// RGridConstructor
public class RGridConstructor : MonoBehaviour {
    
    #region Grid Constructor Variables
    
    // Grid Constructor Variables
    private RGrid<RGridNode> _grid; 

    // Getters and Setters
    public RGrid<RGridNode> Grid {
        get { return _grid; }
        private set {}
    }

    #endregion

    #region Unity Functions

    private void Start() {

        // Initialize Grid
        RGridPreset gridPreset = new RGridPreset();
        _grid = gridPreset.Preset(0);

    }

    #endregion

    #region Debug

    // Gizmos
    public void OnDrawGizmos() {

        // Null check
        if (_grid == null || _grid.Array == null)
            return;

        // Draw Variables
        float s = 0.25f;

        // Loop through grid to draw objects
        foreach (RGridNode node in _grid.Array) {
            
            // Null check
            if (node == null)
                continue;

            // Color
            Gizmos.color = (node.Walkable)? 
                new Color(1, 1, 1, 0.25f) : new Color(0, 0, 0, 0.25f);

            // Convert 
            Vector3 pos = _grid.GetWorld(node.x, node.y, node.z);

            // Draw cube at each point
            Gizmos.DrawCube(pos, new Vector3(s, s, s));
        }
    }

    #endregion

}
