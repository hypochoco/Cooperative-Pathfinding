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

    public void RequestPath(Vector3 targetPoint) {

        foreach (RAgent agent in _agentList) {

            (int startX, int startY, int startZ) = 
                _grid.GetCoord(agent.transform.position);
            (int endX, int endY, int endZ) = 
                _grid.GetCoord(targetPoint);

            Debug.Log("start : (" + startX + ", " + startY + ", " + startZ + ")");
            Debug.Log("end : (" + endX + ", " + endY + ", " + endZ + ")");

            List<RGridNode> pathNodes = 
                _pf.FindPath(startX, startY, startZ, endX, endY + 1, endZ);
            
            if (pathNodes == null)
                return;

            List<Vector3> path = new List<Vector3>();

            foreach (RGridNode node in pathNodes) {

                path.Add(_grid.GetWorld(node.x, node.y, node.z));

            }

            agent.FollowPath(path);

        }

    }

}
