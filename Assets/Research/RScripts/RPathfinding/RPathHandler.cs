using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RPathHandler
public class RPathHandler : MonoBehaviour {
    
    #region RPathHandler Variables

    // RPathHandler Functions
    [SerializeField] private List<RAgent> _agentList;
    [SerializeField] private RGridConstructor _gridConstructor;
    private RResTable<RGridNode> _resTable;
    private RGrid<RGridNode> _grid;
    private RPathfinding _pf;

    #endregion

    #region Unity Functions

    private void Start() {

        // Set initial variables
        _grid = _gridConstructor.Grid;
        _pf = new RPathfinding(_grid);

        // Testing
        _resTable = _grid.RResTable;
        
    }

    #endregion

    #region Testing 

    // Notes:
        // - there should be a way to deal with the whole timing
        //   using the pathindex of each agent


    // public void Test(Vector3 empty) {

    //     // Assign Goals
    //     AssignGoals();

    //     // planning phase
    //     Dictionary<RAgent, List<RGridNode>> pathList = PlanPaths();

    //     // follow paths
    //     FollowPaths(pathList);

    // }


    public void Test(Vector3 empty) {

        // Clear ResTable
        _resTable.Clear();

        // Assign goals
        AssignGoals();

        // while (!AllGoalReached()) {

            Cycle();

        // }
    }

    public void AssignGoals() {
        List<(int, int, int)> targetList = new List<(int, int, int)>() {
            (9, 0, 5),
            (5, 0, 0),
        };
        for (int i = 0; i < _agentList.Count; i++) {
            _agentList[i].Goal = 
                _grid.GetWorld(
                targetList[i].Item1, 
                targetList[i].Item2, 
                targetList[i].Item3);
        }
    }

    public bool AllGoalReached() {
        foreach (var agent in _agentList) {
            if (!agent.GoalReached)
                return false;
        }
        return true;
    }

    public Dictionary<RAgent, List<RGridNode>> PlanPaths() {

        Dictionary<RAgent, List<RGridNode>> pathList = 
            new Dictionary<RAgent, List<RGridNode>>();

        for (int i = 0; i < _agentList.Count; i++) {
            RAgent agent = _agentList[i];

            (int x0, int y0, int z0) = 
                _grid.GetCoord(agent.transform.position);
            (int x1, int y1, int z1) = _grid.GetCoord(agent.Goal);
            List<RGridNode> path = _pf.FindPath(x0, y0, z0, 
                x1, (y1 < 0)? 0 : y1, z1);

            pathList[agent] = path;

        }

        return pathList;

    }

    public (int, RAgent, RAgent, List<RGridNode>) 
        FindFirstConflict(Dictionary<RAgent, List<RGridNode>> pathList) {

        Dictionary<(int, RGridNode), RAgent> travelList = 
            new Dictionary<(int, RGridNode), RAgent>();

        for (int i = 0; i < _agentList.Count; i++) {
            RAgent agent = _agentList[i];

            for (int j = 0; j < pathList[agent].Count; j++) {
                RGridNode node = pathList[agent][j];

                // Testing Purposes
                Debug.Log("Agent: " + i + ", Time: " + j + ", Node: " + node);

                for (int k = -1; k < 1; k++) {
                    try {
                        if (travelList[(j + k, node)] != null) {
                            return (j + k, travelList[(j + k, node)],
                                agent, pathList[agent]);
                        }
                    } catch (KeyNotFoundException) {
                        if (k == 0)
                            travelList[(j, node)] = agent;
                    }
                }

            }

        }

        // No collision found!
        Debug.Log("No collision found!");
        return (-1, null, null, null);

    }

    public void ReserveWindow(int colTime, List<RGridNode> colPath) {
        int windowSize = 3;
        for (int i = -windowSize; i < windowSize + 1; i++) {
            _resTable.Reserve(colPath[colTime + i]);
        }
    }

    public void TruncatePath(int colTime, RAgent agent, ref Dictionary<RAgent, List<RGridNode>> path) {
        int windowSize = 1;
        path[agent] = path[agent].GetRange(0, colTime - windowSize + 1);
    }

    public void FollowPaths(Dictionary<RAgent, List<RGridNode>> pathList) {
        foreach (var entry in pathList) {
            List<Vector3> vectorPathList = new List<Vector3>();
            foreach (var node in entry.Value) {
                vectorPathList.Add(_grid.GetWorld(node.x, node.y, node.z));
            }
            entry.Key.FollowPath(vectorPathList);
        }
    }

    public void Cycle() {

        // planning phase
        Dictionary<RAgent, List<RGridNode>> pathList = PlanPaths();

        // find first conflict
        (int colTime, RAgent agent0, RAgent agent1, List<RGridNode> colPath) = 
            FindFirstConflict(pathList);
        List<RAgent> colAgents = new List<RAgent>();

        // resolve conflicts
        if (colTime != -1) {
            ReserveWindow(colTime, colPath);
            TruncatePath(colTime, agent1, ref pathList);
            colAgents.Add(agent1);
        }

        // Move agents
        FollowPaths(pathList);

        StartCoroutine(Test(colAgents));

    }

    private bool Pathing(List<RAgent> agentList) {
        foreach (var agent in agentList) {
            if (agent.Pathing)  
                return true;
        }
        return false;
    }

    private IEnumerator Test(List<RAgent> colAgents) {

        while (Pathing(colAgents)) {
            yield return null;
        }

        RAgent agent = colAgents[0];

        (int x0, int y0, int z0) = 
            _grid.GetCoord(agent.transform.position);
        (int x1, int y1, int z1) = _grid.GetCoord(agent.Goal);
        List<RGridNode> path = _pf.FindPath(x0, y0, z0, 
            x1, (y1 < 0)? 0 : y1, z1);
        
        List<Vector3> vectorPathList = new List<Vector3>();
        foreach (var node in path) {
            vectorPathList.Add(_grid.GetWorld(node.x, node.y, node.z));
        }
        agent.FollowPath(vectorPathList);
        

        yield break;
    }

    #endregion

}
