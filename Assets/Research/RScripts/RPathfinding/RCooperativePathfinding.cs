using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RCooperativePathfinding : MonoBehaviour{
    
    #region RCooperativePathfinding Variables

    // RCooperativePathfinding Variables
    [SerializeField] private List<RAgent> _agentList;
    private RPathfinding _pf;

    // Testing Purposes
    [SerializeField] private RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;

    #endregion


    #region Unity Functions

    public void Start() {

        // Initialize Variables
        _grid = _gridConstructor.Grid;
        _pf = new RPathfinding(_grid);

    }

    // Main Pathfinding Loop
    public void Update() {

    }

    #endregion


    #region RCooperativePathfinding Functions

    // Testing Purposes
    public void AssignGoals() {
        List<(int, int, int)> targetList = new List<(int, int, int)>() {
            (9, 0, 5),
            (5, 0, 0),
        };
        for (int i = 0; i < _agentList.Count; i++) {
            _agentList[i].Goal = 
                _grid.GetGridItem(
                targetList[i].Item1, 
                targetList[i].Item2, 
                targetList[i].Item3);
        }
    }

    // Plan Paths
    public void PlanPaths(List<RAgent> agentList) {
        foreach (var agent in agentList) {
            RGridNode node = agent.Goal;
            if (node == null)
                continue;

            (int x0, int y0, int z0) = 
                _grid.GetCoord(agent.transform.position);
            List<RGridNode> path = _pf.FindPath(x0, y0, z0, 
                node.x, (node.y < 0)? 0 : node.y, node.z);
            
            agent.FollowPath(path);
        }
    }

    // Collision Detection Function
    public void FindCollisions(out int collisionTime, 
        out List<(RAgent, RAgent)> agentList) {

        // Variables
        Dictionary<(int, RGridNode), RAgent> travelList = 
            new Dictionary<(int, RGridNode), RAgent>();
        collisionTime = -1;
        agentList = new List<(RAgent, RAgent)>();
        
        // Find First Conflict
        foreach (var agent in _agentList) {
            
            // Cach PathIndex
            int pathIndex = agent.PathIndex;

            // Ensure PathIndex
            if (pathIndex == -1)
                continue;

            // Cache Path
            List<RGridNode> path = agent.Path.
                GetRange(pathIndex, agent.Path.Count - pathIndex - 1);

            // Ensure Pathing
            if (path == null)
                continue;
            
            // Search for Collisions
            for (int i = 0; i < path.Count; i++) {

                // Cache Node
                RGridNode node = path[i];
                for (int j = -1; j <= 1; j++) {
                    try {
                        if (travelList[(i + j, node)] != null) {
                            if (i + j < collisionTime) {
                                collisionTime = i + j;
                                agentList.Add((agent, travelList[(i + j, node)]));
                            }
                        }
                    } catch (KeyNotFoundException) {
                        if (j == 0)
                            travelList[(j, node)] = agent;
                    }
                }
            }
        }
    }

    public void ResolveConflict() {

        // reset individual resTables
        // calculate new paths
        // the findpath function should take in a restable or something
        // maybe even move the pathfinding function to individual shits?
        // yert... 
        // get them to folllow the new paths

    }



    #endregion

}
