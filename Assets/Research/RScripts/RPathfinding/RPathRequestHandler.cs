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
        _cpf.AddRAgent(agent);
        _cpf.AssignGoal(agent, targetPositon);
        _cpf.CooperativePathing = true;
    }

    #endregion

    #region Testing Purposes

    // Testing Variables
    [SerializeField] List<RAgent> _agentList;

    // Testing Pathfinding
    public void Test() {

        List<Vector3> targetPositions = new List<Vector3>() {
            new Vector3(4f, 0f, 2f),
            new Vector3(2.5f, 0f, 1f),
        };
        for (int i = 0; i < _agentList.Count; i++) {
            RequestPath(_agentList[i], targetPositions[i]);
        }

    }

    #endregion
}
