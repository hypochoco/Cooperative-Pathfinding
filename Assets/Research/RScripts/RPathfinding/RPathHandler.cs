using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RPathHandler
public class RPathHandler : MonoBehaviour {
    
    #region RPathHandler Variables

    // RPathHandler Functions
    [SerializeField] List<RAgent> _agentList;

    #endregion

    #region RPathHandler Functions

    // Test Function
    public void Test(Vector3 targetPoint) {
        foreach (RAgent agent in _agentList) {
            agent.FollowPath(new List<Vector3>() {targetPoint});
        }
    }

    #endregion

}
