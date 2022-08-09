using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPathHandler : MonoBehaviour {
    
    [SerializeField] RGridConstructor _gridConstructor;
    private RGrid<RGridNode> _grid;
    [SerializeField] private List<RAgent> _agentList;

    private RPathfinding _pf;

    public void Start() {

        StartCoroutine(DelayConstructor());

    }

    IEnumerator DelayConstructor() {

        yield return null;

        _grid = _gridConstructor.Grid;

        _pf = new RPathfinding(_grid);

        yield break;

    }

    public void StartGroupPathFinding() {

        List<Vector3> goalList = new List<Vector3>() {
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
        };

        // TODO : give all the agents a goal
        int i = 0;
        foreach (RAgent agent in _agentList) {
            agent.Goal = goalList[i];
            i++;
        }

        StartCoroutine(GroupPathFindingCoroutine());

    }

    private IEnumerator GroupPathFindingCoroutine() {

        _pf.ResTable.ClearResTable();

        while (!AllGoalsReached()) {

            // plan paths
            foreach (RAgent agent in _agentList) {
                PlanPath(agent, agent.Goal);
            }

            // Find first Conflict





        }

        yield break;

    } 






    private void PlanPath(RAgent agent, Vector3 targetPoint) {

        (int startX, int startY, int startZ) = 
            _grid.GetCoord(agent.transform.position);
        (int endX, int endY, int endZ) = 
            _grid.GetCoord(targetPoint);

        Debug.Log("start : (" + startX + ", " + startY + ", " + startZ + ")");
        Debug.Log("end : (" + endX + ", " + endY + ", " + endZ + ")");

        List<RGridNode> pathNodes = 
            _pf.FindPath(agent, startX, startY, startZ, endX, endY + 1, endZ);
        
        if (pathNodes == null)
            return;

        List<Vector3> path = new List<Vector3>();

        foreach (RGridNode node in pathNodes) {

            path.Add(_grid.GetWorld(node.x, node.y, node.z));

        }

        agent.Path = path;

    }








    // All Agents reached goal
    public bool AllGoalsReached() {
        foreach (RAgent agent in _agentList) {
            if (!agent.GoalReached)
                return false;
        }
        return true;
    }


}
