using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RCooperativePathfinding : MonoBehaviour{
    
    #region RCooperativePathfinding Variables

    // RCooperativePathfinding Variables
    private RPathfinding _pf;
    private List<RAgent> _cpathingRAgents;
    private Queue<RAgent> _ragentQueue;
    private Dictionary<RAgent, RGridNode> _goalList;
    private Dictionary<RAgent, List<RGridNode>> _pathList;
    private Dictionary<RAgent, RResTable<RGridNode>> _resTableList;
    [SerializeField] private RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;
    private bool _cpathing;
    private int _windowSize;
    private int _collisionWindowSize;
    private WaitForSeconds _waitTime;

    #endregion

    #region Unity Functions

    private void Start() {

        // Initialize Variables
        _grid = _gridConstructor.Grid;
        _pf = new RPathfinding(_grid);
        _cpathingRAgents = new List<RAgent>();
        _ragentQueue = new Queue<RAgent>();
        _goalList = new Dictionary<RAgent, RGridNode>();
        _pathList = new Dictionary<RAgent, List<RGridNode>>();
        _resTableList = new Dictionary<RAgent, RResTable<RGridNode>>();
        _cpathing = false;
        _windowSize = 2;
        _collisionWindowSize = 2;
        _waitTime = new WaitForSeconds(1f);

    }

    private void Update() {

        // Do not run cooperative pathfinding
        if (!_cpathing)
            return;

        // Cooperative Pathfinding
        Cycle();
        
    }

    #endregion

    #region RCooperativePathfinding Functions

    // Queue RAgents into pathfinding
    public void RequestCooperativePath(RAgent agent, Vector3 targetPosition) {

        // Find goal
        (int x, int y, int z) = _grid.GetCoord(targetPosition);
        RGridNode targetNode = _grid.GetGridItem(x, (y < 0)? 0 : y, z);
        if (targetNode == null)
            return;

        // Assign Goal
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

        // Update pathing RAgents
        while (_ragentQueue.Count > 0)
            _cpathingRAgents.Add(_ragentQueue.Dequeue());
        if (_cpathingRAgents.Count == 0) {
            _cpathing = false;
            return;
        }
        List<RAgent> cpathingRAgents = _cpathingRAgents;

        // Plan Paths for each agent
        foreach (var agent in cpathingRAgents) {

            // Cache goal
            RGridNode goalNode = _goalList[agent];
            if (goalNode == null)
                continue;

            // Ensure ResTable exists
            if (!_resTableList.ContainsKey(agent))
                _resTableList[agent] = new RResTable<RGridNode>();

            // TODO: ensure cases on and off the grid

            // Singular Pathfinding
            (int x0, int y0, int z0) = 
                _grid.GetCoord(agent.transform.position);
            List<RGridNode> path = _pf.FindPath(_resTableList[agent], 
                x0, y0, z0, 
                goalNode.x, (goalNode.y < 0)? 0 : goalNode.y, goalNode.z);
            if (path == null) {
                StartCoroutine(Wait(agent));
                continue;
            }

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

        // Search for collisions
        foreach (var entry in _pathList) {
            
            // Cache RAgent and path
            RAgent agent = entry.Key;
            List<RGridNode> path = entry.Value;

            // Ensure correct timing
            int pathIndex = agent.PathIndex;
            if (pathIndex >= path.Count)
                continue;
            path = _pathList[agent].
                GetRange(pathIndex, _pathList[agent].Count - pathIndex);
            
            // Search for Collisions per node
            for (int i = 0; i < path.Count; i++) {

                // Cache Node
                RGridNode node = path[i];

                // Testing Purposes
                // Debug.Log("Time: " + i + ", " + node.ToString());

                for (int j = -_collisionWindowSize; j <= _collisionWindowSize; j++) {

                    // Collision exists
                    if (travelList.ContainsKey((i + j, node))) {

                        // Check for first collision
                        if (i + j < collisionTime) {
                            collisionTime = i + j;
                            agentPairList = new List<(RAgent, RAgent)>();
                            agentPairList.Add((agent, travelList[(i + j, node)]));
                        } else if (i + j == collisionTime) {
                            agentPairList.Add((agent, travelList[(i + j, node)]));
                        }
                    } 
                    
                    // Collision doesn't exist
                    else if (j == 0) {
                        travelList[(i, node)] = agent;
                    }
                }
            }
        }

        // Resolve Conflicts

        // Stop cooperative pathfinding if no collisions found
        if (collisionTime == int.MaxValue) {

            // Testing Purposes
            // Debug.Log("Collision not found!");

            // Stop cooperative pathfinding
            _cpathing = false;

            // Reset pathing list
            _cpathingRAgents = new List<RAgent>();
        } 
        
        // Resolve Conflicts
        else { 

            // Testing Purposes
            // Debug.Log("Collision found!");

            // Create new pathingList
            _cpathingRAgents = new List<RAgent>();

            // Iterate through conflicts
            foreach (var agentPair in agentPairList) {

                // Designate conflict owner
                RAgent owner = agentPair.Item1;
                RAgent observer = agentPair.Item2;

                // Assign pathing list
                _cpathingRAgents.Add(observer);

                // Reserve Window
                for (int i = -_windowSize; i <= _windowSize; i++) {
                    _resTableList[observer].
                        Reserve(_pathList[owner][collisionTime + i]);
                }

                // Truncate Path
                int pathCount = (collisionTime - _windowSize - 1 >= 0)?  
                    collisionTime - _windowSize - 1 : 0; 
                _pathList[observer] = 
                    _pathList[observer].GetRange(0, pathCount);
            }
        }

        // Follow Paths

        // Iterate through all paths
        foreach (var agent in cpathingRAgents) {

            // ensure path exists!!!s
            // Convert path
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (var node in _pathList[agent]) {
                vectorPath.Add(_grid.GetWorld(node.x, node.y, node.z));
            }

            // Ensure path exists
            if (vectorPath.Count == 0)
                continue;

            // Follow paths
            agent.FollowPath(vectorPath);
        }
    }

    // Wait if no path found
    private IEnumerator Wait(RAgent agent) {
        yield return _waitTime;
        _ragentQueue.Enqueue(agent);
    }

    #endregion

}
