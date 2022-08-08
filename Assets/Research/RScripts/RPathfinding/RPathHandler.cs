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

        (int startX, int startY, int startZ) = _grid.GetCoord(Vector3.zero);
        (int endX, int endY, int endZ) = _grid.GetCoord(targetPoint);

        Debug.Log("x: " + startX + " y: " + startY + " z: " + startZ);
        Debug.Log("x: " + endX + " y: " + endY + " z: " + endZ);




        List<RGridNode> pathNodes = _pf.FindPath(startX, startY, startZ, endX, 0, endZ);
        
        List<Vector3> path = new List<Vector3>();

        foreach (RGridNode node in pathNodes) {

            path.Add(_grid.GetWorld(node.x, node.y, node.z));

        }

        foreach (RAgent agent in _agentList) {
            agent.FollowPath(path);
        }

    }

}
