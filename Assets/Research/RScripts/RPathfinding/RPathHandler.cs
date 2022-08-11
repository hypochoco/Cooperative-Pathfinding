using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RPathHandler
public class RPathHandler : MonoBehaviour {
    
    #region RPathHandler Variables

    // RPathHandler Functions
    [SerializeField] private List<RAgent> _agentList;
    [SerializeField] private RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;
    private RPathfinding _pf;

    #endregion

    #region Unity Functions

    private void Start() {

        // Set initial variables
        _grid = _gridConstructor.Grid;
        _pf = new RPathfinding(_grid);
    }

    #endregion

    #region RPathHandler Functions

    // Test Function
    public void Test(Vector3 targetPoint) {

        // Grab first RAgent
        RAgent agent = _agentList[0];

        // Calcualte start and end points
        (int x0, int y0, int z0) = _grid.GetCoord(Vector3.zero);
        (int x1, int y1, int z1) = _grid.GetCoord(targetPoint);

        // Find Path
        List<RGridNode> pathNodes = _pf.FindPath(agent, x0, y0, z0, 
            x1, (y1 < 0)? 0 : y1, z1);

        // Ensure path exists
        if (pathNodes == null) {
            Debug.Log("path not found!");
            Debug.Log(_grid.GetCoord(targetPoint));
            return;
        }

        // Convert path
        List<Vector3> path = new List<Vector3>();
        foreach (RGridNode node in pathNodes) {
            path.Add(_grid.GetWorld(node.x, node.y, node.z));
        }

        // Follow Path
        agent.FollowPath(path);

    }

    #endregion

}
