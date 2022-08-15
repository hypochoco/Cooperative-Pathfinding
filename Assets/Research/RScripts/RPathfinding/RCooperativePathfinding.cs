using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RCooperativePathfinding : MonoBehaviour{
    
    #region RCooperativePathfinding Variables

    // RCooperativePathfinding Variables
    private RPathfinding _pf;
    private List<RAgent> _pathingRAgents;
    private Queue<RAgent> _ragentQueue;
    private Dictionary<RAgent, RGridNode> _goalList;
    private Dictionary<RAgent, List<RGridNode>> _pathList;
    private Dictionary<RAgent, RResTable<RGridNode>> _resTableList;
    [SerializeField] private RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;
    private bool _cpathing;
    private int _windowSize;

    #endregion

    #region Unity Functions

    private void Start() {

        // Initialize Variables
        _grid = _gridConstructor.Grid;
        _pf = new RPathfinding(_grid);
        _pathingRAgents = new List<RAgent>();
        _ragentQueue = new Queue<RAgent>();
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

        // Cooperative Pathfinding
        Cycle();
        
    }

    // TODO:
        // - Ensure everything is safe

    #endregion

    #region RCooperativePathfinding Functions

    // Queue RAgents into pathfinding
    public void RequestCooperativePath(RAgent agent, Vector3 targetPosition) {

        // Assign Goal
        (int x, int y, int z) = _grid.GetCoord(targetPosition);
        RGridNode targetNode = _grid.GetGridItem(x, y, z);
        _goalList[agent] = targetNode;

        // Queue Agent
        _ragentQueue.Enqueue(agent);

        // Start Cooperative pathfinding
        _cpathing = true;

    }

    // Remove Agent
    public void RemoveRAgent(RAgent agent) {
        _goalList.Remove(agent);
        _pathList.Remove(agent);
        _resTableList.Remove(agent);
    }

    // Cooperative Pathfinding Cycle
    private void Cycle() {

        // Plan Paths

        // Dequeue RAgents
        while (_ragentQueue.Count > 0) {
            _pathingRAgents.Add(_ragentQueue.Dequeue());
        }
        
        // Cache a list of agents
        List<RAgent> agentList = (_pathingRAgents.Count == 0)? 
            new List<RAgent>(_goalList.Keys) : _pathingRAgents;

        // Plan Paths for each agent
        foreach (var agent in agentList) {

            // Ensure ResTable exists
            if (!_resTableList.ContainsKey(agent))
                _resTableList[agent] = new RResTable<RGridNode>();

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

        // Find First Collision

        // Collision Variables
        int collisionTime = int.MaxValue;
        Dictionary<(int, RGridNode), RAgent> travelList = 
            new Dictionary<(int, RGridNode), RAgent>();
        List<(RAgent, RAgent)> agentPairList = 
            new List<(RAgent, RAgent)>();

        // Find Collisions
        foreach (var agent in agentList) {
            
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
                                agentPairList = new List<(RAgent, RAgent)>();
                                agentPairList.Add((agent, travelList[(i + j, node)]));
                            } else if (i + j == collisionTime) {
                                agentPairList.Add((agent, travelList[(i + j, node)]));
                            }
                        }
                    } catch (KeyNotFoundException) {
                        if (j == 0)
                            travelList[(i, node)] = agent;
                    }
                }
            }
        }

        // Resolve Conflicts

        // Stop cooperative pathfinding if no collisions found
        if (collisionTime == int.MaxValue) {
            _cpathing = false;
        } 
        
        // Resolve Conflicts
        else { 

            // Iterate through conflicts
            foreach (var agentPair in agentPairList) {

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

        // Follow Paths

        // Iterate through all paths
        foreach (var entry in _pathList) {

            // Convert path
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (var node in entry.Value) {
                vectorPath.Add(_grid.GetWorld(node.x, node.y, node.z));
            }

            // Ensure path exists
            if (vectorPath.Count == 0)
                continue;

            // Follow paths
            entry.Key.FollowPath(vectorPath);
        }
    }

    #endregion

}
