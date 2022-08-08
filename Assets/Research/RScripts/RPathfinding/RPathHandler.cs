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


    public void RequestPath(Vector3 _targetPoint) {
        
        

    }



}
