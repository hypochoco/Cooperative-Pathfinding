using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RCooperativePathfinding : MonoBehaviour{
    
    #region RCooperativePathfinding Variables

    // RCooperativePathfinding Variables
    private RPathfinding _pf;
    private List<RAgent> _agentList; 
    private Dictionary<RAgent, RGridNode> _goalList;
    private Dictionary<RAgent, List<RGridNode>> _pathList;
    private Dictionary<RAgent, RResTable<RGridNode>> _resTableList;
    [SerializeField] private RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;
    private bool _cpathing;
    private int _windowSize;

    // Getters and Setters
    public bool CooperativePathing {
        get { return _cpathing; }
        set { _cpathing = value; }
    }

    #endregion

    #region Unity Functions

    private void Start() {

        // Initialize Variables
        _grid = _gridConstructor.Grid;
        _pf = new RPathfinding(_grid);
        _agentList = new List<RAgent>();
        _goalList = new Dictionary<RAgent, RGridNode>();
        _pathList = new Dictionary<RAgent, List<RGridNode>>();
        _resTableList = new Dictionary<RAgent, RResTable<RGridNode>>();
        _cpathing = false;
        _windowSize = 2;

    }

    private void Update() {

        // Do not run cooperative pathfinding
        if (!_cpathing)
            return;
        
        // Wait until truncated path has been reached...
        

        // Cooperative Pathfinding

        // Plan Paths
        PlanPaths(_agentList);

        // Find Conflicts
        FindCollisions(out int collisionTime, 
            out List<(RAgent, RAgent)> agentList);

        // Stop if no collisions found, else resolve conflicts
        if (collisionTime == int.MaxValue) {
            _cpathing = false;
        } else { // Resolve Conflicts
            ResolveConflicts(collisionTime, agentList);
        }

        // Follow Paths
        FollowPaths();
        
    }

    // TODO:
        // - Remove Agents after they reached their goal
        // - Better handle this whole agentsList
        // - Ensure everything is safe
        // - Remove things that aren't needed
        //   _pathList, _goalList, _resTables

    #endregion

    #region RCooperativePathfinding Functions

    // Add agent to pathfinding
    public void AddRAgent(RAgent agent) {
        _agentList.Add(agent);
        _resTableList[agent] = new RResTable<RGridNode>();
    }

    // Assign Goals
    public void AssignGoal(RAgent agent, Vector3 targetPosition) {
        (int x, int y, int z) = _grid.GetCoord(targetPosition);
        RGridNode targetNode = _grid.GetGridItem(x, y, z);
        _goalList[agent] = targetNode;
    }

    // Plan Paths
    private void PlanPaths(List<RAgent> agentList) {
        foreach (var agent in agentList) {

            // Cache goal
            RGridNode goalNode = _goalList[agent];
            
            // Skip if no goal
            if (goalNode == null)
                continue;

            // TODO: ensure cases on and off the grid

            // Singular Pathfinding
            (int x0, int y0, int z0) = 
                _grid.GetCoord(agent.transform.position);
            List<RGridNode> path = _pf.FindPath(_resTableList[agent], 
                x0, y0, z0, 
                goalNode.x, (goalNode.y < 0)? 0 : goalNode.y, goalNode.z);

            // Ensure path found
            if (path == null)
                continue;

            // Store path
            _pathList[agent] = path;
        }
    }

    // Follow Paths
    private void FollowPaths() {
        foreach (var entry in _pathList) {

            // Convert path
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (var node in entry.Value) {
                vectorPath.Add(_grid.GetWorld(node.x, node.y, node.z));
            }

            // Ensure path exists
            if (vectorPath.Count == 0)
                continue;

            // follow paths
            entry.Key.FollowPath(vectorPath);
        }
    }

    // Collision Detection Function
    private void FindCollisions(out int collisionTime, 
        out List<(RAgent, RAgent)> agentList) {

        // Collision Variables
        Dictionary<(int, RGridNode), RAgent> travelList = 
            new Dictionary<(int, RGridNode), RAgent>();
        collisionTime = int.MaxValue;
        agentList = new List<(RAgent, RAgent)>();
        
        // Find First Conflict
        foreach (var agent in _agentList) {
            
            // Cach PathIndex
            int pathIndex = agent.PathIndex;

            // Ensure PathIndex
            if (pathIndex >= _pathList[agent].Count)
                continue;

            // Cache Path
            List<RGridNode> path = _pathList[agent].
                GetRange(pathIndex, _pathList[agent].Count - pathIndex);

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
                                agentList = new List<(RAgent, RAgent)>();
                                agentList.Add((agent, travelList[(i + j, node)]));
                            } else if (i + j == collisionTime) {
                                agentList.Add((agent, travelList[(i + j, node)]));
                            }
                        }
                    } catch (KeyNotFoundException) {
                        if (j == 0)
                            travelList[(i, node)] = agent;
                    }
                }
            }
        }
    }

    // Resolve Conflicts
    private void ResolveConflicts(int collisionTime, List<(RAgent, RAgent)> agentList) {
        // Resolve Conflicts
        foreach (var agentPair in agentList) {

            // Reserve Window
            for (int i = -_windowSize; i <= _windowSize; i++) {
                _resTableList[agentPair.Item2].
                    Reserve(_pathList[agentPair.Item1][collisionTime + i]);
            }
            
            // Truncate Path
            int pathCount = (collisionTime - _windowSize - 1 >= 0)?  
                collisionTime - _windowSize - 1 : 0; 
            _pathList[agentPair.Item2] = 
                _pathList[agentPair.Item2].GetRange(0, pathCount);
        }
    }

    #endregion

}
