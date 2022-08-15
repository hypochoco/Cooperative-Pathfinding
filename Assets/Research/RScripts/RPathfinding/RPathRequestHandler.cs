using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPathRequestHandler : MonoBehaviour {
    
    #region RPathRequestHandler Variables

    // RPathRequestHandler Variables
    [SerializeField] private RCooperativePathfinding _cpf;

    #endregion

    #region  RPathRequestHandler Functions

    // Request a path
    public void RequestPath(RAgent agent, Vector3 targetPositon) {
        _cpf.RequestCooperativePath(agent, targetPositon);
    }

    #endregion

    #region Testing Purposes

    // Testing Variables
    [SerializeField] List<RAgent> _agentList;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Q)) {
            Test(0);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Test(1);
        }

    }

    public void Test() {
        
    }

    // Testing Pathfinding
    public void Test(int n) {

        List<Vector3> targetPositions = new List<Vector3>() {
            new Vector3(4f, 0f, 2f),
            new Vector3(2.5f, 0f, 1f),
        };
        RequestPath(_agentList[n], targetPositions[n]);

    }

    #endregion
}
